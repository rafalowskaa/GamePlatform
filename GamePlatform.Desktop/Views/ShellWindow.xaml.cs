using GamePlatform.Desktop.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace GamePlatform.Desktop.Views;

public partial class ShellWindow : Window
{
    public ShellViewModel ShellVM { get; }

    public ShellWindow(ShellViewModel vm)
    {
        ShellVM = vm;
        InitializeComponent();
        DataContext = ShellVM;
    }
}
