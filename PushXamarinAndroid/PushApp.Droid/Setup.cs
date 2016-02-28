using Android.Content;
using Gcm;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using PushApp.Core;
using PushApp.Core.Utiles;

namespace PushApp.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
            RegistrarApp(applicationContext);
        }

        protected override IMvxApplication CreateApp()
        {
            return new App();
        }

        private void RegistrarApp(Context applicationContext)
        {
            GcmClient.CheckDevice(applicationContext);
            GcmClient.CheckManifest(applicationContext);

            GcmClient.Register(applicationContext, Constantes.SenderId);
        }
    }
}