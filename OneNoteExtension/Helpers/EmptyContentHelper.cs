using LinqToOneNote;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Commands;
using OneNoteExtension.Pages;
using OneNoteExtension.Pages.Core;
using OneNoteExtension.Properties;
using System.Linq;

namespace OneNoteExtension.Helpers;

internal static class EmptyContentHelper
{
    public static CommandItem NoMatchesFound { get; } = new()
    {
        Title = Resources.NoMatchesFound,
        Icon = Icons.Logo
    };

    public static CommandItem GetNotMatchesFoundWithCommands(IExternalItemsChanged listPage, IOneNoteItem item) => new()
    {
        Title = NoMatchesFound.Title,
        Icon = NoMatchesFound.Icon,
        Subtitle = Resources.SeeMoreCommands,
        MoreCommands = PageHelper.GetMoreCommands(item, listPage, true).ToContextItems()
    };

    public static CommandItem GetNotMatchesFoundWithCommands(IExternalItemsChanged listPage, Root root) => new()
    {
        Title = NoMatchesFound.Title,
        Icon = NoMatchesFound.Icon,
        Subtitle = Resources.SeeMoreCommands,
        MoreCommands =
        [
            new OpenOneNoteCommand().ToContextItem(),
            new CreateItemFormPage.Notebook(root, listPage).ToContextItem()
        ]
    };

    public static CommandItem EmptySearch { get; } = new()
    {
        Title = Resources.SearchPageDefaultEmptyContent,
        Icon = Icons.Logo,
        Subtitle = Resources.SeeMoreCommands,
        MoreCommands =
        [
            new OpenOneNoteCommand().ToContextItem(),
            ..TopLevelCommands.Commands.Skip(1).Select(p => p.ToContextItem()),
        ]
    };

    public static CommandItem InvalidSearch { get; } = new()
    {
        Title = Resources.InvalidSearch,
        Icon = Icons.Invalid
    };
}