using System;
using System.Globalization;
using System.Text.Json.Nodes;
using LinqToOneNote;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Helpers;
using OneNoteExtension.Properties;
namespace OneNoteExtension.Pages;

internal partial class QuickNoteFormPage : ContentPage
{
    public QuickNoteFormContent form = new();

    public QuickNoteFormPage()
    {
        Title = Resources.CreateQuickNote;
        Name = Resources.CreateOneNotePage;
        Icon = Icons.NewPage;
    }

    public override IContent[] GetContent() => [form];

    public partial class QuickNoteFormContent : FormContent
    {
        public QuickNoteFormContent()
        {
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
            "text": " ${Labels.title}",
            "horizontalAlignment": "Center",
            "wrap": true,
            "style": "heading"
        },
        {
            "type": "Input.Text",
            "id": "name",
            "errorMessage": "Name cannot contain the characters",
            "placeholder": "${Labels.name}",
            "regex": ""
        },
        {
            "type": "Input.Text",
            "isMultiline": true,
            "id": "content",
            "placeholder": "${Labels.content}\n"
        },
        {
            "type": "ActionSet",
            "actions": [
                {
                    "type": "Action.Submit",
                    "title": "${Labels.createAction}",
                    "data" : {
                        "openOneNote" : false
                    }
                },
                {
                    "type": "Action.Submit",
                    "title": "${Labels.createOpenAction}",
                    "data" : {
                        "openOneNote" : true
                    }
                }
            ]
        }
    ]
}
""";
            DataJson =  $$"""
{
    "Labels" : {
        "title" : "{{Resources.CreateQuickNote}}",
        "name" : "{{Resources.NewItemNamePlaceholder}}",
        "content" : "{{Resources.PageContentPlaceholder}}",
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

            var pageName = formInput["name"]?.ToString();
            var pageContent = formInput["content"]?.ToString();
            _ = bool.TryParse(formData["openOneNote"]?.ToString(), out var showOneNote);

            OneNoteHelper.CreateQuickNote(pageName, pageContent, showOneNote);
            if (showOneNote)
            {
                return CommandResult.Dismiss();
            }
            else
            {
                return CommandResult.ShowToast(new ToastArgs
                {
#pragma warning disable CA1863
                    Message = string.Format(CultureInfo.CurrentCulture, Resources.CreatedNewItemInOneNote, pageName),
#pragma warning restore CA1863
                    Result = CommandResult.GoBack()
                });
            }
        }
    }
}
