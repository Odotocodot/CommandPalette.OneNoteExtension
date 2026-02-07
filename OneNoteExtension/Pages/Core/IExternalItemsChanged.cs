namespace OneNoteExtension.Pages.Core;

//For ListPages where the items have changed externally. For instance, when creating a notebook with the CreateItemForm, OneNoteExplorerRootPage need to be updated.
internal interface IExternalItemsChanged
{
    public void RaiseItemsChangedExternal(); //Maybe a better name... that said, could do with some betters names in a lot of places.
}
