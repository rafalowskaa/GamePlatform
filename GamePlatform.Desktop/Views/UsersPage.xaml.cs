using System.Windows.Controls;

namespace GamePlatform.Desktop.Views;

public partial class UsersPage : Page
{
    public UsersPage() => InitializeComponent();

    private void OnPrintClick(object sender, System.Windows.RoutedEventArgs e)
    {
        var dlg = new System.Windows.Controls.PrintDialog();
        if (dlg.ShowDialog() == true)
            dlg.PrintVisual(UsersGrid, "Users");
    }
}
