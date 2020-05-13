using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Plugin.LocalNotification;

namespace MyNotes.Droid
{
    [Activity(Label = "MyNotes", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            // Для корректной работы элемента SwipeView.
            Xamarin.Forms.Forms.SetFlags("SwipeView_Experimental");
            // Определяем сервис.
            Intent startServiceIntent = new Intent(this, typeof(MyNotesService));
            startServiceIntent.SetAction("MyNotes.action.START_SERVICE");
            // Для операционных систем Андроид 8.0 и выше.
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                // Создаем канал уведомления.
                NotificationCenter.CreateNotificationChannel(
                    new Plugin.LocalNotification.Platform.Droid.NotificationChannelRequest
                    {
                        Id = $"myNotificationsChannel",
                        Name = "General",
                        Description = "General",
                    });
                // Запускаем сервис.
                StartForegroundService(startServiceIntent);
            }
            else
                StartService(startServiceIntent);
            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
    }
}