using GamePlatform.Desktop.Services;
using System.Windows;
using System.Windows.Controls;

namespace GamePlatform.Desktop.Views;

public partial class NotificationCenterPage : Page
{
    public NotificationCenterPage()
    {
        InitializeComponent();
    }
    private void RemoveItem_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is not INotificationService svc)
            return;

        if (sender is Button btn && btn.DataContext is string msg)
            svc.History.Remove(msg);
    }
}
