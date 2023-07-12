using AniMoe.App.Services;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Helpers
{
    public class NotificationService : INotificationService
    {
        private AppNotification notification;
        public void SendImageNotification(string path, string imageUrl)
        {
            notification = new AppNotificationBuilder()
                .AddText("Image Downloaded Successfully")
                .AddText($"Location: {path}")
                .SetAppLogoOverride(new Uri(path), AppNotificationImageCrop.Circle)
                .BuildNotification();
            AppNotificationManager.Default.Show(notification);
            Log.Debug("Notification sent successfully");
        }
    }
}
