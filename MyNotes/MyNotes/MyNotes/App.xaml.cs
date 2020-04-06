using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyNotes.Pages;
using System;

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
            //NotificationsPage.CreateSystemNotifications();
        }

        protected override void OnSleep()
        {
            NotificationsPage.CreateSystemNotifications(DateTime.Now.DayOfWeek.ToString());
        }

        protected override void OnResume()
        {
            NotificationsPage.CreateSystemNotifications(DateTime.Now.DayOfWeek.ToString());
        }
    }
}
