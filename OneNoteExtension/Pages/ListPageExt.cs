using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Windows.Foundation;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace OneNoteExtension.Pages;

// Based on https://github.com/microsoft/PowerToys/blob/87c65f9eec76faf9b1f34b062eeced4ac2e59a18/src/modules/cmdpal/ext/SamplePagesExtension/OnLoadPage.cs 
// Allows for subscribing when a page is Loaded/Unloaded
internal abstract partial class ListPageExt : Page, IListPage
{
    private string _searchText = string.Empty;
    private ICommandItem? _emptyContent;
    private IFilters? _filters;
    private IGridProperties? _gridProperties;
    private bool _hasMoreItems;
    private string _placeholderText = string.Empty;
    private bool _showDetails;
    public event Action? PageLoaded;
    public event Action? PageUnloaded;
    private event TypedEventHandler<object, IItemsChangedEventArgs>? InternalItemsChanged;
    public event TypedEventHandler<object, IItemsChangedEventArgs> ItemsChanged
    {
        add
        {
            InternalItemsChanged += value;
            PageLoaded?.Invoke();
        }

        remove
        {
            InternalItemsChanged -= value;
            PageUnloaded?.Invoke();
        }
    }

    public virtual ICommandItem? EmptyContent { get => _emptyContent; set => SetProperty(ref _emptyContent, value); }
    public virtual IFilters? Filters { get => _filters; set => SetProperty(ref _filters, value); }
    public virtual IGridProperties? GridProperties { get => _gridProperties; set => SetProperty(ref _gridProperties, value); }
    public virtual bool HasMoreItems { get => _hasMoreItems; set => SetProperty(ref _hasMoreItems, value); }
    public virtual string PlaceholderText { get => _placeholderText; set => SetProperty(ref _placeholderText, value); }
    public virtual string SearchText { get => _searchText; set => SetProperty(ref _searchText, value); }
    public virtual bool ShowDetails { get => _showDetails; set => SetProperty(ref _showDetails, value); }

    protected void SetProperty<T>(ref T field, in T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return;
        }

        field = value;
        OnPropertyChanged(propertyName!);
    }
    public abstract IListItem[] GetItems();
    public virtual void LoadMore() { }
    protected void RaiseItemsChanged(int totalItems = -1)
    {
        try
        {
            InternalItemsChanged?.Invoke(this, new ItemsChangedEventArgs(totalItems));
        }
        catch
        {
            // ignored
        }
    }
    protected void SetSearchNoUpdate(string newSearchText) => _searchText = newSearchText;
}