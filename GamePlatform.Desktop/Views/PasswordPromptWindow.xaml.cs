using System.Windows;

namespace GamePlatform.Desktop.Views;

public partial class PasswordPromptWindow : Window
{
    public PasswordPromptWindow() => InitializeComponent();

    public string Password => PwdBox.Password;

    private void OkClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Password))
        {
            MessageBox.Show("Hasło nie może być puste.",
                            "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        DialogResult = true;
    }
}
