using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GamePlatform.Desktop.ViewModels;

namespace GamePlatform.Desktop.Views;

public partial class LoginWindow : Window
{
    public LoginWindow() => InitializeComponent();

    /* ---- przekazywanie haseł ---- */
    private void PwdChanged(object s, RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel vm && s is PasswordBox pb)
            vm.Password = pb.Password;
    }
    private void PwdVisibleChanged(object s, TextChangedEventArgs e)
    {
        if (DataContext is LoginViewModel vm && s is TextBox tb)
            vm.Password = tb.Text;
    }
    private void PwdChangedRegister(object s, RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel vm && s is PasswordBox pb)
            vm.RegPassword = pb.Password;
    }
    private void RegPwdVisibleChanged(object s, TextChangedEventArgs e)
    {
        if (DataContext is LoginViewModel vm && s is TextBox tb)
            vm.RegPassword = tb.Text;
    }
    private void PwdChangedRepeat(object s, RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel vm && s is PasswordBox pb)
            vm.RegPasswordRepeat = pb.Password;
    }
    private void RegRepeatVisibleChanged(object s, TextChangedEventArgs e)
    {
        if (DataContext is LoginViewModel vm && s is TextBox tb)
            vm.RegPasswordRepeat = tb.Text;
    }

    /* ---- Enter = Zatwierdź ---- */
    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Return) return;
        if (DataContext is not LoginViewModel vm) return;

        if (vm.IsRegistering)
            vm.SubmitRegisterCommand.Execute(null);
        else
            vm.SubmitLoginCommand.Execute(this);
    }

    /* ---- przełączanie paneli ---- */
    private void ShowRegisterClick(object _, RoutedEventArgs __)
        => ((LoginViewModel)DataContext!).IsRegistering = true;
    private void ShowLoginClick(object _, RoutedEventArgs __)
        => ((LoginViewModel)DataContext!).IsRegistering = false;

    private void ForgotPwdClick(object _, RoutedEventArgs __)
        => MessageBox.Show("Reset hasła – TODO.", "Info",
                           MessageBoxButton.OK, MessageBoxImage.Information);
}
