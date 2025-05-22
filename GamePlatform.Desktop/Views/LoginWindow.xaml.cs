using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GamePlatform.Desktop.ViewModels;

namespace GamePlatform.Desktop.Views;

public partial class LoginWindow : Window
{
    public LoginWindow() => InitializeComponent();

    /* ========== LOGOWANIE: hasło ========== */
    private void PwdChanged(object s, RoutedEventArgs _)
    {
        if (DataContext is LoginViewModel vm && s is PasswordBox pb)
            vm.Password = pb.Password;
    }
    private void PwdVisibleChanged(object s, TextChangedEventArgs _)
    {
        if (DataContext is LoginViewModel vm && s is TextBox tb)
            vm.Password = tb.Text;
    }

    /* ========== REJESTRACJA: hasło ========== */
    private void PwdChangedRegister(object s, RoutedEventArgs _)
    {
        if (DataContext is LoginViewModel vm && s is PasswordBox pb)
            vm.RegPassword = pb.Password;
    }
    private void RegPwdVisibleChanged(object s, TextChangedEventArgs _)
    {
        if (DataContext is LoginViewModel vm && s is TextBox tb)
            vm.RegPassword = tb.Text;
    }
    private void PwdChangedRepeat(object s, RoutedEventArgs _)
    {
        if (DataContext is LoginViewModel vm && s is PasswordBox pb)
            vm.RegPasswordRepeat = pb.Password;
    }

    /* ========== ENTER = Akcja domyślna ========== */
    private void OnKeyDown(object? _, KeyEventArgs e)
    {
        if (e.Key != Key.Return || DataContext is not LoginViewModel vm) return;
        if (vm.IsRegistering)
            vm.SubmitRegisterCommand.Execute(null);
        else
            vm.SubmitLoginCommand.Execute(this);
    }

    /* ========== PRZEŁĄCZANIE PANELI ========== */
    private void ShowRegisterClick(object _, RoutedEventArgs __)
        => ((LoginViewModel)DataContext!).IsRegistering = true;

    private void ShowLoginClick(object _, RoutedEventArgs __)
        => ((LoginViewModel)DataContext!).IsRegistering = false;
}
