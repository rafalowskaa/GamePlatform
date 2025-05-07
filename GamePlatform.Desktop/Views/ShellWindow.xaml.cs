using System.Windows;
using GamePlatform.Desktop.ViewModels;

namespace GamePlatform.Desktop.Views;
public partial class ShellWindow : Window
{
    public ShellWindow(ShellViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}
