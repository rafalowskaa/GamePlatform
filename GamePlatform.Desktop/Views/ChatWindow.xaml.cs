using GamePlatform.Desktop.Services;
using GamePlatform.Desktop.ViewModels;
using GamePlatform.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;

namespace GamePlatform.Desktop.Views;

public partial class ChatWindow : Window
{
    public ChatWindow() => InitializeComponent();

    private void TextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && (Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
        {
            if(DataContext is ChatWindowViewModel vm && vm.SendCommand.CanExecute(null))
                vm.SendCommand.Execute(null);
            e.Handled = true;
        }
    }

    /* ---------- SINGLETON-like otwieranie ---------- */
    public static void Open(User me, User mate)
    {
        foreach (Window w in Application.Current.Windows)
            if (w is ChatWindow cw && cw.DataContext is ChatWindowViewModel vm &&
                vm.Me.Id == me.Id && vm.Mate.Id == mate.Id)
            { cw.Activate(); return; }

        var chatSvc = App.Current.Services.GetRequiredService<IChatService>();
        new ChatWindow
        {
            DataContext = new ChatWindowViewModel(chatSvc, me, mate),
            Owner = Application.Current.MainWindow
        }.Show();
    }
}
