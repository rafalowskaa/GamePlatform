using System.Windows.Controls;
using System.Windows;

namespace GamePlatform.Desktop.Views;

public partial class AdminReviewPage : Page
{
    public AdminReviewPage() => InitializeComponent();

    private void OnPrintClick(object sender, RoutedEventArgs e)
    {
        var dlg = new PrintDialog();
        if (dlg.ShowDialog() == true)
            dlg.PrintVisual(GamesGrid, "Games");
    }
}