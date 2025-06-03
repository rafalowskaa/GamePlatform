using System.Windows.Controls;
using System.Windows;
using GamePlatform.Desktop.Utils;

namespace GamePlatform.Desktop.Views;

public partial class AdminReviewPage : Page
{
    public AdminReviewPage() => InitializeComponent();

    private void OnPrintClick(object sender, RoutedEventArgs e)
    {
        Utils.PrintHelper.PrintDataGrid(GamesGrid, "Games");
    }
}