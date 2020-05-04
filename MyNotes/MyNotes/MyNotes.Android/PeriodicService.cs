using Android.App;
using Android.Content;
using Android.OS;
using Xamarin.Forms;
using System;

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
            Device.StartTimer(new TimeSpan(00, 00, 00), () =>
            {
                MessagingCenter.Send<string>("CreateSystemNotifications", GetTomorrow());
                return true;
            });

            return StartCommandResult.RedeliverIntent;
        }

        

    }
}