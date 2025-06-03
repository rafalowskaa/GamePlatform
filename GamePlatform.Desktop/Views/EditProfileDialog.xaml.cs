using System.Windows;
using System.Windows.Controls;
using GamePlatform.Desktop.ViewModels;

namespace GamePlatform.Desktop.Views;

public partial class EditProfileDialog : Window
{
    public EditProfileDialog() => InitializeComponent();

    private void OnPwdChanged(object s, RoutedEventArgs e)
    {
        if (DataContext is ProfileViewModel vm && s is PasswordBox pb)
            vm.NewPassword = pb.Password;
    }

    private async void OnSaveClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is ProfileViewModel vm)
        {
            await vm.SaveCommand.ExecuteAsync(null);
            if (string.IsNullOrEmpty(vm.Error))
                DialogResult = true;
        }
    }
}