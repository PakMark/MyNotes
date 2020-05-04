using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;


namespace MyNotes.Droid
{
    //public class NotificationWorker : Worker
    //{
    //    public NotificationWorker(Context context, WorkerParameters workerParameters) : base(context, workerParameters)
    //    {

    //    }
    //    /// <summary>
    //    /// Метод определения завтрашнего дня
    //    /// </summary>
    //    /// <returns>Завтрашний день</returns>
    //    string GetTomorrow()
    //    {
    //        int numberToday = (int)DateTime.Now.DayOfWeek;
    //        string[] days = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
    //        return days[(numberToday++) % 7];
    //    }
    //    public override Result DoWork()
    //    {
    //        var taxReturn = CalculateTaxes();
    //        MyNotes.Pages.NotificationsPage.CreateSystemNotifications(GetTomorrow());
    //        return Result.InvokeSuccess();
    //    }

    //    public double CalculateTaxes()
    //    {
    //        return 2000;
    //    }
    //}
}

