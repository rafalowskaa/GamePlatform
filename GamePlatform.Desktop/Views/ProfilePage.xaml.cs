using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GamePlatform.Desktop.ViewModels;
using GamePlatform.Desktop.Localization;

namespace GamePlatform.Desktop.Views;
public partial class ProfilePage : Page
{
    public ProfilePage() => InitializeComponent();

    private void PwdChanged(object s, RoutedEventArgs e)
    {
        if (DataContext is ProfileViewModel vm && s is PasswordBox pb)
            vm.NewPassword = pb.Password;
    }

    private void OnAvatarDrop(object sender, DragEventArgs e)
    {
        if (DataContext is not ProfileViewModel vm) return;
        if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

        var files = (string[])e.Data.GetData(DataFormats.FileDrop);
        if (files.Length == 0) return;

        string path = files[0];
        var ext = System.IO.Path.GetExtension(path).ToLowerInvariant();
        var fi = new System.IO.FileInfo(path);

        if (fi.Length > 1_000_000 || (ext != ".jpg" && ext != ".png"))
        {
            MessageBox.Show(Localization.TRaw("FILE_INVALID"), Localization.TRaw("ERROR"),
                            MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        vm.AvatarPath = path;
        vm.Avatar = new System.Windows.Media.Imaging.BitmapImage(new System.Uri(path));
    }
}
