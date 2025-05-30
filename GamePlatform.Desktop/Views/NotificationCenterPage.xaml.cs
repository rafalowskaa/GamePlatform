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
    private void RemoveSelected_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is INotificationService svc && List.SelectedItem is string msg)
            svc.History.Remove(msg);
    }
}