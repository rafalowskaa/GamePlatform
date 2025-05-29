using GamePlatform.Desktop.ViewModels;
using GamePlatform.Shared.Models;
using System.Windows.Controls;
using System.Windows.Input;

namespace GamePlatform.Desktop.Views;

public partial class FriendsPage : Page
{
    public FriendsPage() => InitializeComponent();

    private void OnSearchEnter(object? _, KeyEventArgs e)
    {
        if (e.Key == Key.Return && DataContext is FriendListViewModel vm)
            vm.RefreshSuggestionsCommand.Execute(null);
    }

    private void OnSuggestionClick(object? sender, MouseButtonEventArgs _)
    {
        if (sender is not ListBox lb || lb.SelectedItem is not User u) return;
        if (DataContext is not FriendListViewModel vm) return;

        vm.StartChatCommand.Execute(u);
    }
}
