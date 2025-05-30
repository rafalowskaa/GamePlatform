using GamePlatform.Desktop.ViewModels;
using GamePlatform.Shared.Models;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Controls.Primitives;

namespace GamePlatform.Desktop.Views;

public partial class FriendsPage : UserControl
{
    public FriendsPage() => InitializeComponent();

    /* ENTER w polu wyszukiwania */
    private void OnSearchEnter(object? _, KeyEventArgs e)
    {
        if (e.Key == Key.Return && DataContext is FriendListViewModel vm)
            vm.RefreshSuggestionsCommand.Execute(null);
    }

    /* ===== PODPOWIEDŹ (lewy klik) ===== */
    private void OnSuggestionItemClick(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton != MouseButton.Left) return;
        if (sender is not ListBoxItem item) return;

        if (item.ContextMenu != null)
        {
            item.ContextMenu.DataContext = DataContext;   // <── KLUCZ
            item.ContextMenu.PlacementTarget = item;
            item.ContextMenu.Placement = PlacementMode.MousePoint;
            item.ContextMenu.IsOpen = true;
            e.Handled = true;
        }
    }

    /* ===== ZNAJOMY (lewy klik) ===== */
    private void OnFriendItemClick(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton != MouseButton.Left) return;
        if (sender is not ListViewItem item) return;

        if (item.ContextMenu != null)
        {
            item.ContextMenu.DataContext = DataContext;   // <── KLUCZ
            item.ContextMenu.PlacementTarget = item;
            item.ContextMenu.Placement = PlacementMode.MousePoint;
            item.ContextMenu.IsOpen = true;
            e.Handled = true;
        }
    }

    private void OnBlockedItemClick(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton != MouseButton.Left) return;
        if (sender is not ListViewItem item) return;

        if (item.ContextMenu != null)
        {
            item.ContextMenu.DataContext = DataContext;
            item.ContextMenu.PlacementTarget = item;
            item.ContextMenu.Placement = PlacementMode.MousePoint;
            item.ContextMenu.IsOpen = true;
            e.Handled = true;
        }
    }

    /* podwójny klik ⇒ start czatu */
    private void OnFriendOpen(object? _, MouseButtonEventArgs e)
    {
        if (e.ChangedButton != MouseButton.Left) return;
        if (DataContext is FriendListViewModel vm && vm.SelectedFriend is User u)
            vm.StartChatCommand.Execute(u);
    }

    /* blokuje domyślne menu po prawym kliku */
    private void DisableContextMenu(object? _, ContextMenuEventArgs e)
        => e.Handled = true;
}
  