using Android.App;
using Android.Content;
using Android.OS;
using Xamarin.Forms;
using System;
using Android.Widget;

namespace MyNotes.Droid
{
    [Service]
    public class PeriodicService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        /// <summary>
        /// Метод определения завтрашнего дня
        /// </summary>
        /// <returns>Завтрашний день</returns>
        string GetTomorrow()
        {
            int numberToday = (int)DateTime.Now.DayOfWeek;
            string[] days = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            return days[(numberToday++) % 7];
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Device.StartTimer(TimeSpan.FromMinutes(1), () =>
            {
                MessagingCenter.Send<string>("CreateSystemNotifications", DateTime.Now.DayOfWeek.ToString());
                return true;
            });

            return StartCommandResult.Sticky;
        }

        

    }
}