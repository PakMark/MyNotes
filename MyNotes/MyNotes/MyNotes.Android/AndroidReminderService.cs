﻿using System;
using Android.Content;
using Android.App;
using Xamarin.Forms;
using MyNotes.Models;
using Android.OS;

[assembly: Xamarin.Forms.Dependency(typeof(MyNotes.Droid.AndroidReminderService))]

namespace MyNotes.Droid
{
    public class AndroidReminderService : IReminderService
    {
        public void Remind(DateTime dateTime, string title, string message)
        {

            Intent alarmIntent = new Intent(Forms.Context, typeof(AlarmReceiver));
            alarmIntent.PutExtra("message", message);
            alarmIntent.PutExtra("title", title);

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(Forms.Context, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
            AlarmManager alarmManager = (AlarmManager)Forms.Context.GetSystemService(Context.AlarmService);

            //TODO: For demo set after 5 seconds.
            alarmManager.Set(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + 5 * 1000, pendingIntent);


        }
    }
}