using System.Windows;

namespace GamePlatform.Desktop.Views
{
    public partial class PlayWindow : Window
    {
        public PlayWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.PlayWindowViewModel vm)
            {
                Title = vm.Title;
            }
        }
    }
}
