using System.Windows.Controls;
using GamePlatform.Desktop.Utils;

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
            Utils.PrintHelper.PrintDataGrid(ScoresGrid, "Stats");
        }
    }
}
