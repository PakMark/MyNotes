using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using MyNotes.Models;
using MyNotes.Days;
using Plugin.LocalNotifications;

namespace MyNotes.Pages
{
    delegate Task<List<Notification>> GetTodayNotification();

    public partial class NotificationsPage : ContentPage
    {
        static Dictionary<string, GetTodayNotification> days = new Dictionary<string, GetTodayNotification>()
        {
            { "Monday", () => Monday.Database.GetNotificationAsync()},
            { "Tuesday", () => Tuesday.Database.GetNotificationAsync()},
            { "Wednesday", () => Wednesday.Database.GetNotificationAsync()},
            { "Thursday", () => Thursday.Database.GetNotificationAsync()},
            { "Friday", () => Friday.Database.GetNotificationAsync()},
            { "Saturday", () => Saturday.Database.GetNotificationAsync()},
            { "Sunday", () => Sunday.Database.GetNotificationAsync()}
        };

        public NotificationsPage()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Метод генерации уведомлений на устройстве
        /// </summary>
        public static async void CreateSystemNotifications()
        {
            List<Notification> todayNotifications;
            string today = DateTime.Now.DayOfWeek.ToString();
            var notifications = await days[today]();
            // Получение сегодняшних напоминаний и передеча в listView.
            todayNotifications = notifications.OrderBy(X => X.NotificationTime)
                                              .OrderByDescending(X => X.IsNotify)
                                              .TakeWhile(X => X.IsNotify == true)
                                              .ToList();

            foreach (var notification in todayNotifications)
            {
                // Создание времени уведомления.
                DateTime notificationTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                    DateTime.Now.Day, notification.NotificationTime.Hours,
                    notification.NotificationTime.Minutes, notification.NotificationTime.Seconds);
                if (notificationTime > DateTime.Now)
                    CrossLocalNotifications.Current.Show("Уведомление MyNotes", notification.FormatTime,
                        notification.ID, notificationTime);
            }
        }

        async private void MondayClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Days.Monday());
        }

        async private void TuesdayClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Days.Tuesday());
        }

        async private void WednesdayClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Days.Wednesday());
        }

        async private void ThursdayClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Days.Thursday());
        }

        async private void FridayClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Days.Friday());
        }

        async private void SaturdayClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Days.Saturday());
        }

        async private void SundayClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Days.Sunday());
        }

        async private void SettingsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Days.Settings());
        }
    }
}