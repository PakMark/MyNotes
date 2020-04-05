using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using MyNotes.Models;
using MyNotes.Days;
using Plugin.LocalNotifications;

namespace MyNotes
{
    delegate Task<List<Notification>> GetTodayNotification();

    public partial class MainPage : ContentPage
    {
        Dictionary<string, GetTodayNotification> days = new Dictionary<string, GetTodayNotification>()
        {
            { "Monday", () => Monday.Database.GetNotificationAsync()},
            { "Tuesday", () => Tuesday.Database.GetNotificationAsync()},
            { "Wednesday", () => Wednesday.Database.GetNotificationAsync()},
            { "Thursday", () => Thursday.Database.GetNotificationAsync()},
            { "Friday", () => Friday.Database.GetNotificationAsync()},
            { "Saturday", () => Saturday.Database.GetNotificationAsync()},
            { "Sunday", () => Sunday.Database.GetNotificationAsync()}
        };
        
        public MainPage()
        {
            InitializeComponent();
            UpdateNowNotifications();
        }

        /// <summary>
        /// Метод повторного обновления сегодняшних напоминаний
        /// </summary>
        public void UpdateNowNotifications()
        {
            string today = DateTime.Now.DayOfWeek.ToString();
            todayLabel.Text = today;
            GetTodayNotifications(today);
        }

        /// <summary>
        /// Метод получения сегодняшних напоминаний
        /// </summary>
        /// <param name="today">День недели</param>
        async void GetTodayNotifications(string today)
        {
            var notifications = await days[today]();
            // Получение сегодняшних напоминаний и передеча в listView.
            todayNotifications = notifications.OrderBy(X => X.NotificationTime)
                                              .OrderByDescending(X => X.IsNotify)
                                              .TakeWhile(X => X.IsNotify == true)
                                              .ToList();
            listView.ItemsSource = todayNotifications;
        }
        
        /// <summary>
        /// Список напоминаний на сегодня
        /// </summary>
        List<Notification> todayNotifications;

        /// <summary>
        /// Метод вызова уведомления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotificationButtonClicked(object sender, EventArgs e)
        {
            foreach(var notification in todayNotifications)
            {
                CrossLocalNotifications.Current.Show("Уведомление MyNotes", notification.ToString(),
                    notification.ID);
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
