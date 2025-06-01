using GamePlatform.Desktop.ViewModels;
using GamePlatform.Shared.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GamePlatform.Desktop.Views;

public partial class GamesPage : Page
{
    public GamesPage() => InitializeComponent();

    private void OnDoubleClick(object s, MouseButtonEventArgs e)
    {
        if (DataContext is MainViewModel vm && vm.SelectedGame is Game g)
            vm.PlayCommand.Execute(g);
    }

    private void OnTileClick(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is MainViewModel vm && sender is FrameworkElement fe && fe.DataContext is Game g)
            vm.SelectedGame = g;
    }
}
