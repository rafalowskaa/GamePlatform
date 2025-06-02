using System.Windows;
using GamePlatform.Desktop.Localization;
using Loc = GamePlatform.Desktop.Localization.Localization;

namespace GamePlatform.Desktop.Views;

public partial class PasswordPromptWindow : Window
{
    public PasswordPromptWindow() => InitializeComponent();

    public string Password => PwdBox.Password;

    private void OkClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Password))
        {
            MessageBox.Show(Loc.TRaw("EMPTY_PASSWORD"),
                            Loc.TRaw("ERROR"), MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        DialogResult = true;
    }
}
