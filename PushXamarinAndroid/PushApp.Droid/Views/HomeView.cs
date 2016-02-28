using Android.App;
using Android.Widget;
using MvvmCross.Droid.Views;
using PushApp.Core.ViewModels;

namespace PushApp.Droid.Views
{
    [Activity(Label = "Almacén de smartphones", MainLauncher = true)]
    public class HomeView : MvxActivity
    {
        public new HomeViewModel ViewModel
        {
            get { return (HomeViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            
            SetContentView(Resource.Layout.Home);
        }
    }
}