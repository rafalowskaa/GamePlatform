using GamePlatform.Desktop.ViewModels;
using GamePlatform.Shared.Models;
using System.Windows.Controls;
using System.Windows.Input;

namespace GamePlatform.Desktop.Views;

public partial class FriendsPage : UserControl
{
    public FriendsPage() => InitializeComponent();

    private void OnSearchEnter(object? _, KeyEventArgs e)
    {
        if (e.Key == Key.Return && DataContext is FriendListViewModel vm)
            vm.RefreshSuggestionsCommand.Execute(null);
    }

    private void OnSuggestionClick(object? sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton != MouseButton.Left) return;
        if (sender is not ListBox lb || lb.SelectedItem is not User u) return;


        if (lb.ItemContainerGenerator.ContainerFromItem(u) is ListBoxItem item)
        {
            if (item.ContextMenu != null)
            {
                item.ContextMenu.DataContext = u;
                item.ContextMenu.IsOpen = true;
            }
        }
    }

    private void OnFriendOpen(object? sender, MouseButtonEventArgs e)
    {
        if (DataContext is FriendListViewModel vm && vm.SelectedFriend is User u && e.ChangedButton == MouseButton.Left)
            vm.StartChatCommand.Execute(u);
    }
}
