using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using LinqToOneNote;
using LinqToOneNote.Abstractions;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Helpers;
using OneNoteExtension.Properties;

namespace OneNoteExtension.Pages;

internal partial class CreateItemFormPage : ContentPage
{
    private readonly CreateItemFormContent _form;

    protected CreateItemFormPage(IconInfo icon, in CreateData createData)
    {
        _form = new CreateItemFormContent(createData);

        Title = createData.title;
        Icon = icon;
        Name = Resources.Create;
    }

    public override IContent[] GetContent() => [_form];

    public partial class QuickNote() : CreateItemFormPage(
        Icons.NewPage,
        new CreateData(
            Resources.CreateQuickNote,
            false,
            null,
            OneNoteHelper.CreateQuickNote));

    public partial class Page(LinqToOneNote.Section parent) : CreateItemFormPage(
        Icons.NewPage,
        new CreateData(
            Resources.CreateOneNotePage,
            false,
            null,
            (name, content, openMode) => OneNoteHelper.CreatePage(name, content, parent, openMode)));

    public partial class Section(INotebookOrSectionGroup parent) : CreateItemFormPage(
        Icons.NewSection,
        new CreateData(
            Resources.CreateOneNoteSection,
            true,
            LinqToOneNote.Section.InvalidCharacters,
            (name, _, openMode) => OneNoteHelper.CreateSection(name, parent, openMode)));

    public partial class SectionGroup(INotebookOrSectionGroup parent) : CreateItemFormPage(
        Icons.NewSectionGroup,
        new CreateData(
            Resources.CreateOneNoteSectionGroup,
            true,
            LinqToOneNote.SectionGroup.InvalidCharacters,
            (name, _, openMode) => OneNoteHelper.CreateSectionGroup(name, parent, openMode)));

    public partial class Notebook(Root root) : CreateItemFormPage(
        Icons.NewNotebook,
        new CreateData(
            Resources.CreateOneNoteNotebook,
            true,
            LinqToOneNote.SectionGroup.InvalidCharacters,
            (name, _, openMode) => OneNoteHelper.CreateNotebook(name, root, openMode)));

    protected delegate void CreateAction(string name, string? content, OpenMode openMode);
    protected readonly struct CreateData(string title, bool nameRequired, IReadOnlyList<char>? invalidChars, CreateAction createAction)
    {
        public readonly string title = title;
        public readonly string nameRequired = nameRequired.ToString().ToLowerInvariant();
        public readonly string nameRegex = invalidChars == null ? string.Empty : JsonEncodedText.Encode($@"^(?!\s+$)[^{string.Concat(invalidChars)}]+$").Value;
#pragma warning disable CA1863
        public readonly string nameErrorMessage = JsonEncodedText.Encode(string.Format(CultureInfo.CurrentCulture, Resources.NameCannotContainChars, string.Join(" ", invalidChars ?? []))).Value;
#pragma warning restore CA1863
        public readonly CreateAction createAction = createAction;
        public readonly string showPageContent = (!nameRequired).ToString().ToLowerInvariant();
    }

    private partial class CreateItemFormContent : FormContent
    {
        private readonly CreateData _createData;
        public CreateItemFormContent(in CreateData createData)
        {
            _createData = createData;
            TemplateJson = /*lang=json,strict*/ """
{
    "$schema": "http://adaptivecards.io/schemas/adaptive-card.json",
    "type": "AdaptiveCard",
    "version": "1.6",
    "body": [
        {
            "type": "TextBlock",
            "size": "Medium",
            "weight": "Bolder",
            "text": "${data.title}",
            "horizontalAlignment": "Center",
            "wrap": true,
            "style": "heading"
        },
        {
            "type": "Input.Text",
            "id": "name",
            "errorMessage": "${data.nameError}",
            "placeholder": "${data.name}",
            "regex": "${data.nameValidation}",
            "label": "${data.name}",
            "isRequired": "${data.nameRequired}"
        },
        {
            "type": "Input.Text",
            "isMultiline": true,
            "id": "content",
            "placeholder": "${data.content}\n",
            "separator": true,
            "label": "${data.content}",
            "$when": "${data.showContent}"
        },
        {
            "type": "ActionSet",
            "actions": [
                {
                    "type": "Action.Submit",
                    "title": "${data.createAction}",
                    "data": {
                        "openOneNote": "false"
                    },
                    "associatedInputs": "auto"
                },
                {
                    "type": "Action.Submit",
                    "title": "${data.createOpenAction}",
                    "data": {
                        "openOneNote": "true"
                    },
                    "associatedInputs": "auto"
                }
            ]
        }
    ]
}
""";
            DataJson = $$"""
{
    "data" : {
        "title" : "{{createData.title}}",
        "name" : "{{Resources.Name}}",
        "nameError" : "{{createData.nameErrorMessage}}",
        "nameValidation" : "{{createData.nameRegex}}",
        "nameRequired" : {{createData.nameRequired}},
        "showContent" : {{createData.showPageContent}},
        "content" : "{{Resources.PageContent}}",
        "createAction" : "{{Resources.Create}}",
        "createOpenAction" : "{{Resources.CreateOpen}}"
    }
}
""";
        }

        public override CommandResult SubmitForm(string inputs, string data)
        {
            var formData = JsonNode.Parse(data)?.AsObject();
            var formInput = JsonNode.Parse(inputs)?.AsObject();
            if (formInput == null || formData == null)
            {
                return CommandResult.GoHome();
            }

            var name = formInput["name"]?.ToString().Trim();
            var content = formInput["content"]?.ToString();
            _ = bool.TryParse(formData["openOneNote"]?.ToString(), out var showOneNote);

            _ = Task.Run(() => _createData.createAction(name ?? string.Empty, content, showOneNote ? OpenMode.ExistingOrNewWindow : OpenMode.None));

            if (showOneNote)
            {
                return CommandResult.Dismiss();
            }

            return CommandResult.ShowToast(new ToastArgs
            {
#pragma warning disable CA1863
                Message = string.Format(CultureInfo.CurrentCulture, Resources.CreatedNewItemInOneNote, name),
#pragma warning restore CA1863
                Result = CommandResult.GoBack()
            });
        }
    }
}
