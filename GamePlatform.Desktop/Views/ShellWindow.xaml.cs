using GamePlatform.Desktop.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace GamePlatform.Desktop.Views;

public partial class ShellWindow : Window
{
    public ShellViewModel ShellVM { get; }

    public ShellWindow(ShellViewModel vm)
    {
        ShellVM = vm;
        InitializeComponent();
        DataContext = ShellVM;
    }

    private void OnTabChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (sender is System.Windows.Controls.TabControl tc && tc.SelectedItem is System.Windows.Controls.TabItem item)
        {
            string tag = item.Tag as string ?? string.Empty;
            var vm = ShellVM;
            switch (tag)
            {
                case "Games": vm.ShowGamesCommand.Execute(null); break;
                case "Profile": vm.ShowProfileCommand.Execute(null); break;
                case "Stats": vm.ShowStatsCommand.Execute(null); break;
                case "Notify": vm.ShowNotificationsCommand.Execute(null); break;
                case "Pending": vm.ShowPendingCommand.Execute(null); break;
                case "Reports": vm.ShowReportsCommand.Execute(null); break;
                case "Users": vm.ShowUsersCommand.Execute(null); break;
            }
        }
    }
}
