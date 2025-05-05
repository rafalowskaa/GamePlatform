using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GamePlatform.Shared.Models;
using GamePlatform.Desktop.ViewModels;

namespace GamePlatform.Desktop;

public partial class MainWindow : Window
{
    public MainWindow() { InitializeComponent(); GameList.MouseDoubleClick += OnDbl; }

    private void OnDbl(object sender, MouseButtonEventArgs e)
    {
        if (GameList.SelectedItem is Game g && DataContext is MainViewModel vm)
            vm.PlayCommand.Execute(g);
    }

    private void OnCtx(object sender, ContextMenuEventArgs e)
    {
        if (GameList.ContextMenu is not null) return;
        var m = new ContextMenu();
        var del = new MenuItem { Header = "Usuń" };
        del.Click += (_, _) => { if (DataContext is MainViewModel vm && GameList.SelectedItem is Game g) vm.DeleteCommand.Execute(g); };
        m.Items.Add(del);
        GameList.ContextMenu = m;
    }
}