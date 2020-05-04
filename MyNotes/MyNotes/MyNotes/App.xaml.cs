using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyNotes.NotePages;
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
                    new NavigationPage(new NotesPage())
                    {
                        Title = "Заметки"
                    },
                    new NavigationPage(new MemoriesPage())
                    {
                        Title = "Записи"
                    }
                }

            };

            MessagingCenter.Subscribe<string>(this, "CreateSystemNotifications" , (e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    NotesPage.CreateSystemNotifications(e);
                });
            });
        }

        protected override void OnStart()
        {
            //NotesPage.CreateSystemNotifications();
        }

        protected override void OnSleep()
        {
            NotesPage.CreateSystemNotifications(DateTime.Now.DayOfWeek.ToString());
        }

        protected override void OnResume()
        {
            NotesPage.CreateSystemNotifications(DateTime.Now.DayOfWeek.ToString());
        }
    }
}
