using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Networking.PushNotifications;
using Microsoft.WindowsAzure.Messaging;

namespace PushUniversalWindows.Servicios
{
    public class ServicioPush : IServicioPush
    {
        private readonly string _notificationHubPath;
        private readonly string _connectionString;
        private readonly IEnumerable<string> _tags;

        public ServicioPush(string notificationHubPath, string connectionString, IEnumerable<string> tags)
        {
            _notificationHubPath = notificationHubPath;
            _connectionString = connectionString;
            _tags = tags;
        }

        public async Task<Registration> Registrar()
        {
            var canal = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            var hub = new NotificationHub(_notificationHubPath, _connectionString);

            return await hub.RegisterNativeAsync(canal.Uri, _tags);
        }
    }
}