using System.Windows;
using GamePlatform.Desktop.ViewModels;

namespace GamePlatform.Desktop.Views;

/// <summary>Code-behind okna głównego.</summary>
public partial class ShellWindow : Window
{
    public ShellWindow(ShellViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}
