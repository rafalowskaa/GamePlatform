using System.Windows.Controls;
using System.Windows.Input;
using GamePlatform.Shared.Models;
using GamePlatform.Desktop.ViewModels;

namespace GamePlatform.Desktop.Views;

public partial class GamesPage : Page
{
    public GamesPage() => InitializeComponent();

    private void OnDoubleClick(object s, MouseButtonEventArgs e)
    {
        if (DataContext is MainViewModel vm && vm.SelectedGame is Game g)
            vm.PlayCommand.Execute(g);
    }
}
