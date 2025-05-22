using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GamePlatform.Desktop.ViewModels;
using GamePlatform.Shared.Models;

namespace GamePlatform.Desktop.Views;

public partial class FriendsPage : Page
{
    public FriendsPage() => InitializeComponent();

    /* ENTER w polu wyszukiwania */
    private void OnKey(object? _, KeyEventArgs e)
    {
        if (e.Key == Key.Return && DataContext is FriendListViewModel vm)
            vm.FindSuggestionsCommand.Execute(null);
    }

    /* wybór sugestii z ComboBoxa */
    private void OnSuggestPicked(object? sender, RoutedEventArgs _)
    {
        if (DataContext is not FriendListViewModel vm) return;
        if (sender is not ComboBoxItem cbi || cbi.DataContext is not User u) return;

        vm.SelectedSuggestion = u;
        vm.InviteCommand.Execute(u);   // wysyła zaproszenie + komunikat
    }
}
