using MyNotes.Models;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace MyNotes.NotePages
{
    public class MyNotifications
    {
        /// <summary>
        /// Метод генерации уведомлений на устройстве на сегодняшний день
        /// </summary>
        /// <param name="day">День напоминания</param>
        public static async void GenerateNotifications(string today)
        {
            string day = DateTime.Now.DayOfWeek.ToString();
            if (day == today)
            {
                // Получение списка заметок на определенный день.
                var notes = await NotesPage.days[day]();
                // Генерация уведомлений.
                CreateSystemNotifications(notes, DateTime.Now);
            }
        }

        /// <summary>
        /// Метод генерации уведомлений на устройстве на сегодня и на завтра
        /// </summary>
        /// <param name="today">Сегодняшний день</param>
        /// <param name="tomorrow">Завтрашний день</param>
        public static async void GenerateNotifications(string today, string tomorrow)
        {
            // Генерация уведомлений на сегодняшний день.
            GenerateNotifications(today);
            var notes = await NotesPage.days[tomorrow]();
            DateTime nextDay = DateTime.Now.AddDays(1);
            // Инициализация уведомлений на завтрашний день.
            CreateSystemNotifications(notes, new DateTime(nextDay.Year, nextDay.Month,
                nextDay.Day, 0, 0, 0));

        }

        /// <summary>
        /// Метод инициализации уведомлений на устройстве
        /// </summary>
        /// <param name="notes">Список заметок</param>
        /// <param name="dayTime">День напоминания</param>
        public static void CreateSystemNotifications(List<Note> notes, DateTime dayTime)
        {
            List<Note> todayEnabledNotes;
            List<Note> todayDisabledNotes;
            // Получение сегодняшних заметок, о которых нужно напомнить.
            todayEnabledNotes = notes.OrderBy(X => X.NotificationTime)
                                     .OrderByDescending(X => X.IsNotify)
                                     .TakeWhile(X => X.IsNotify == true)
                                     .ToList();

            // Получение сегодняшних заметок, о которых не нужно напоминать.
            todayDisabledNotes = notes.Except(todayEnabledNotes)
                                      .ToList();

            // Создание уведомлений.
            foreach (var note in todayEnabledNotes)
            {
                // Создание времени уведомления.
                DateTime notificationTime = new DateTime(dayTime.Year, dayTime.Month,
                    dayTime.Day, note.NotificationTime.Hours, note.NotificationTime.Minutes, 0, 0);
                // Проверка времени.
                if (notificationTime >= dayTime)
                {
                    var notification = new NotificationRequest
                    {
                        NotificationId = note.ID,
                        Title = "Уведомление MyNotes",
                        Description = $"{note.FormatTime} {note.NoteText}",
                        NotifyTime = notificationTime
                    };
                    if (DependencyService.Get<IAndroidVersion>().IsHigherOreo())
                        notification.Android.ChannelId = "myNotificationsChannel";
                    NotificationCenter.Current.Show(notification);
                }
            }

            // Удаление уведомлений.
            foreach (var note in todayDisabledNotes)
            {
                NotificationCenter.Current.Cancel(note.ID);
            }
        }
    }
}
