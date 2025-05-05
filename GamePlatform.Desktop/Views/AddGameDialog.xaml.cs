using System.Windows;
using GamePlatform.Desktop.ViewModels;

namespace GamePlatform.Desktop.Views;

public partial class AddGameDialog : Window
{
    public AddGameDialog() => InitializeComponent();

    private void OnSubmit(object sender, RoutedEventArgs e)
    {
        if (DataContext is AddGameDialogViewModel vm && vm.Validate())
            DialogResult = true;
    }
}