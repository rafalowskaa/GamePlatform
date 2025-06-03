using System.Windows.Controls;

namespace GamePlatform.Desktop.Views
{
    /// <summary>
    /// Pusty code-behind – tylko po to,
    /// aby typ dziedziczył z <see cref="Page"/>.
    /// </summary>
    public partial class StatsPage : Page
    {
        public StatsPage() => InitializeComponent();

        private void OnPrintClick(object sender, System.Windows.RoutedEventArgs e)
        {
            var dlg = new System.Windows.Controls.PrintDialog();
            if (dlg.ShowDialog() == true)
                dlg.PrintVisual(ScoresGrid, "Stats");
        }
    }
}
