using System.Collections.Generic;
using WindowsAzure.Messaging;
using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Gcm.Client;
using Newtonsoft.Json;
using PushApp.Core.Model;
using PushApp.Core.Utiles;

[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]
[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]

namespace PushApp.Droid.Servicios
{
    [BroadcastReceiver(Permission = Constants.PERMISSION_GCM_INTENTS)]
    [IntentFilter(new[] { Constants.INTENT_FROM_GCM_MESSAGE }, Categories = new[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new[] { Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK }, Categories = new[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new[] { Constants.INTENT_FROM_GCM_LIBRARY_RETRY }, Categories = new[] { "@PACKAGE_NAME@" })]
    public class BroadcastReceiver : GcmBroadcastReceiverBase<PushHandlerService>
    {
        public static string[] SenderIds = { Constantes.SenderId };
    }

    [Service]
    public class PushHandlerService : GcmServiceBase
    {
        public static string RegistrationId { get; private set; }
        private NotificationHub Hub { get; set; }

        public PushHandlerService() : base(BroadcastReceiver.SenderIds) { }

        protected override void OnMessage(Context context, Intent intent)
        {
            string mensaje = intent.Extras.GetString("mensaje");
            var smartphone = JsonConvert.DeserializeObject<Smartphone>(mensaje);

            var texto =
                "Modelo: " + smartphone.Modelo +
                "\nFabricante: " + smartphone.Fabricante +
                "\nPrecio: " + smartphone.Precio + " €";

            LanzarNotificacion("Nuevo smartphone", texto);
        }

        protected override void OnRegistered(Context context, string registrationId)
        {
            RegistrationId = registrationId;
            Hub = new NotificationHub(Constantes.NotificationHubPath, Constantes.ConnectionString, context);

            var tags = new List<string>() { "NuevoSmartphone" };

            Hub.Register(registrationId, tags.ToArray());
        }

        protected override void OnError(Context context, string errorId) { }
        protected override void OnUnRegistered(Context context, string registrationId) { }

        private static void LanzarNotificacion(string title, string desc)
        {
            var context = Application.Context;

            NotificationCompat.Builder builder = new NotificationCompat.Builder(context)
                .SetAutoCancel(true)
                .SetSmallIcon(Resource.Drawable.Icon)
                .SetStyle(new NotificationCompat.BigTextStyle().BigText(desc))
                .SetContentTitle(title)
                .SetContentText(desc);

            var notificationManager = (NotificationManager)context.GetSystemService(NotificationService);

            notificationManager.Notify(17774, builder.Build());
        }
    }
}