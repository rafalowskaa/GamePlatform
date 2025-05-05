// Views/LoginWindow.xaml.cs
using GamePlatform.Desktop.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace GamePlatform.Desktop.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            // spinamy / wypinamy event tylko, gdy DataContext się zmieni
            DataContextChanged += (_, e) =>
            {
                if (e.OldValue is LoginViewModel oldVm)
                    oldVm.Authenticated -= OnAuthenticated;

                if (e.NewValue is LoginViewModel newVm)
                    newVm.Authenticated += OnAuthenticated;
            };
        }

        /* ========= handlers pól haseł ========= */
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

        /* ========= przełączanie paneli ========= */
        private void ShowRegisterClick(object s, RoutedEventArgs e)
            => ((LoginViewModel)DataContext!).IsRegistering = true;

        private void ShowLoginClick(object s, RoutedEventArgs e)
            => ((LoginViewModel)DataContext!).IsRegistering = false;

        /* ========= placeholder „zapomniałem hasła” ========= */
        private void ForgotPwdClick(object s, RoutedEventArgs e)
            => MessageBox.Show(
                "Przypominanie hasła pojawi się w kolejnej wersji aplikacji.",
                "Info", MessageBoxButton.OK, MessageBoxImage.Information);

        /* ========= sukces logowania / rejestracji ========= */
        private void OnAuthenticated()
        {
            // absolutnie **żadnego** DialogResult – tylko zamykamy okno
            Close();
        }
    }
}
