using System.Windows;
using System.Windows.Controls;
using GamePlatform.Desktop.ViewModels;

namespace GamePlatform.Desktop.Views;
public partial class ProfilePage : Page
{
    public ProfilePage() => InitializeComponent();

    private void PwdChanged(object s, RoutedEventArgs e)
    {
        if (DataContext is ProfileViewModel vm && s is PasswordBox pb)
            vm.NewPassword = pb.Password;
    }
}
