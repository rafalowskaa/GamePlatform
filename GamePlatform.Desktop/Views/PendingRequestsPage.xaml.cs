using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GamePlatform.Desktop.Views
{
    /// <summary>
    /// Logika interakcji dla klasy PendingRequestsPage.xaml
    /// </summary>
    public partial class PendingRequestsPage : Page
    {
        public PendingRequestsPage()
        {
            InitializeComponent();
        }

        private void OnPrintClick(object sender, RoutedEventArgs e)
        {
            var dlg = new PrintDialog();
            if (dlg.ShowDialog() == true)
                dlg.PrintVisual(RequestsGrid, "Requests");
        }
    }
}
