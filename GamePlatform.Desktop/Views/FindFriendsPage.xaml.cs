using System.Windows.Controls;
using System.Windows.Input;
using GamePlatform.Desktop.ViewModels;

namespace GamePlatform.Desktop.Views;

public partial class FindFriendsPage : Page
{
    public FindFriendsPage() => InitializeComponent();

    /* naciśnięcie Enter w polu wyszukiwania */
    private void OnKey(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Return && DataContext is FindFriendsViewModel vm)
            vm.SearchCommand.Execute(null);
    }
}
