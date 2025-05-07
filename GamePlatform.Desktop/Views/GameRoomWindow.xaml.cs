using System.Windows;
using System.Windows.Input;
using GamePlatform.Desktop.ViewModels;

namespace GamePlatform.Desktop.Views;
public partial class GameRoomWindow : Window
{
    public GameRoomWindow(GameRoomViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }

    private void OnKeyDown(object s, KeyEventArgs e)
    {
        if (e.Key != Key.Return) return;
        if (DataContext is GameRoomViewModel vm)
            vm.SendCommand.Execute(null);
        
    }
}
