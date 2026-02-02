using Microsoft.CommandPalette.Extensions;

namespace OneNoteExtension.Pages
{
    internal abstract partial class DynamicListPageExt : ListPageExt, IDynamicListPage
    {
        public override string SearchText
        {
            get => base.SearchText;
            set
            {
                SetSearchNoUpdate(value);
                UpdateSearchText(base.SearchText, value);
            }
        }

        public abstract void UpdateSearchText(string oldSearch, string newSearch);
    }
}
