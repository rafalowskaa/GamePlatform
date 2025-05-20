using System.Windows.Controls;
using System.Windows.Input;
using GamePlatform.Desktop.ViewModels;


namespace GamePlatform.Desktop.Views;

public partial class FriendsPage : Page
{
    public FriendsPage() => InitializeComponent();

    private void OnKey(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Return && DataContext is FriendListViewModel vm)
            vm.FindSuggestionsCommand.Execute(null);
    }
}
