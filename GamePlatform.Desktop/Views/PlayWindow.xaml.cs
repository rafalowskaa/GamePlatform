using System.Windows;
using GamePlatform.Desktop.ViewModels;

namespace GamePlatform.Desktop.Views;

public partial class PlayWindow : Window
{
    public PlayWindow()
    {
        InitializeComponent();
        if (DataContext is PlayWindowViewModel vm)
            Title = vm.Title;
    }
}