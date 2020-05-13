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


        }

        protected override void OnStart()
        {
            MyNotifications.GenerateNotifications(DateTime.Now.DayOfWeek.ToString(), GetTomorrow());
            MessagingCenter.Subscribe<string>(this, "CreateSystemNotifications", (e) =>
            {
                MyNotifications.GenerateNotifications(e, GetTomorrow());
            });
        }

        protected override void OnSleep()
        {
            MyNotifications.GenerateNotifications(DateTime.Now.DayOfWeek.ToString(), GetTomorrow());
        }

        protected override void OnResume()
        {
            // Handle when your app resumes.
        }

        /// <summary>
        /// Метод определения завтрашнего дня
        /// </summary>
        /// <returns>Завтрашний день</returns>
        string GetTomorrow()
        {
            int numberToday = (int)DateTime.Now.DayOfWeek;
            string[] days = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            return days[(++numberToday) % 7];
        }
    }
}
