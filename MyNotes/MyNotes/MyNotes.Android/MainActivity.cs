using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Plugin.LocalNotifications;
using System;

namespace MyNotes.Droid
{
    [Activity(Label = "MyNotes", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            Xamarin.Forms.Forms.SetFlags("SwipeView_Experimental");
            //StartService(new Intent(this, typeof(PeriodicService)));
            LocalNotificationsImplementation.NotificationIconId = Resource.Drawable.icon;
            // PeriodicWorkRequest taxWorkRequest = PeriodicWorkRequest.Builder.From<NotificationWorker>(new TimeSpan(00,00,00)).Build();
            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

        }
    }
}