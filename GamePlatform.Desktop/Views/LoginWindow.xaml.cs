using System.Windows;
using System.Windows.Controls;
using GamePlatform.Desktop.ViewModels;

namespace GamePlatform.Desktop.Views;
public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();

        DataContextChanged += (_, e) =>
        {
            if (e.OldValue is LoginViewModel oldVm)
                oldVm.Authenticated -= OnAuthenticated;
            if (e.NewValue is LoginViewModel newVm)
                newVm.Authenticated += OnAuthenticated;
        };
    }

    /* --- obsługa haseł --- */
    private void PwdChanged(object s, RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel vm && s is PasswordBox pb)
            vm.Password = pb.Password;
    }
    private void PwdChangedRegister(object s, RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel vm && s is PasswordBox pb)
            vm.RegPassword = pb.Password;
    }
    private void PwdChangedRepeat(object s, RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel vm && s is PasswordBox pb)
            vm.RegPasswordRepeat = pb.Password;
    }

    /* --- przełączanie paneli --- */
    private void ShowRegisterClick(object s, RoutedEventArgs e) => ((LoginViewModel)DataContext!).IsRegistering = true;
    private void ShowLoginClick(object s, RoutedEventArgs e) => ((LoginViewModel)DataContext!).IsRegistering = false;

    private void ForgotPwdClick(object s, RoutedEventArgs e) =>
        MessageBox.Show("Reset hasła – TODO.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

    private void OnAuthenticated() => Close();
}
