using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyNotes.Pages;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MyNotes
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage()
            {
                Children =
                {
                    new NavigationPage(new Pages.NotificationsPage())
                    {
                        Title = "Notifications"
                    },
                    new NavigationPage(new Pages.NotesPage())
                    {
                        Title = "Notes"
                    }
                }

            };
        }

        protected override void OnStart()
        {
            NotificationsPage.CreateSystemNotifications();
        }

        protected override void OnSleep()
        {
            NotificationsPage.CreateSystemNotifications();
        }

        protected override void OnResume()
        {
            NotificationsPage.CreateSystemNotifications();
        }
    }
}
