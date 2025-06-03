using System.Windows.Controls;
using GamePlatform.Desktop.Utils;

namespace GamePlatform.Desktop.Views;

public partial class UsersPage : Page
{
    public UsersPage() => InitializeComponent();

    private void OnPrintClick(object sender, System.Windows.RoutedEventArgs e)
    {
        Utils.PrintHelper.PrintDataGrid(UsersGrid, "Users");
    }
}
