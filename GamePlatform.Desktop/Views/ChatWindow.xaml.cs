using GamePlatform.Desktop.Services;
using GamePlatform.Desktop.ViewModels;
using GamePlatform.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;
using System;
using System.Collections.Specialized;

namespace GamePlatform.Desktop.Views;

public partial class ChatWindow : Window
{
    public ChatWindow()
    {
        InitializeComponent();
        Loaded += ChatWindow_Loaded;
    }

    protected override void OnClosed(EventArgs e)
    {
        if (DataContext is ChatWindowViewModel vm)
            vm.Messages.CollectionChanged -= Messages_CollectionChanged;
        if (DataContext is IDisposable d)
            d.Dispose();
        base.OnClosed(e);
    }

    private void ChatWindow_Loaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is ChatWindowViewModel vm)
            vm.Messages.CollectionChanged += Messages_CollectionChanged;
    }

    private void Messages_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (History.Items.Count > 0)
            History.ScrollIntoView(History.Items[^1]);
    }

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
