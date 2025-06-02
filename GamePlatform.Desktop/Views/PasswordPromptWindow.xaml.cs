using System.Windows;
using GamePlatform.Desktop.Localization;

namespace GamePlatform.Desktop.Views;

public partial class PasswordPromptWindow : Window
{
    public PasswordPromptWindow() => InitializeComponent();

    public string Password => PwdBox.Password;

    private void OkClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Password))
        {
            MessageBox.Show(Localization.TRaw("EMPTY_PASSWORD"),
                            Localization.TRaw("ERROR"), MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        DialogResult = true;
    }
}
