using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using MyNotes.Models;
using MyNotes.Days;
using Plugin.LocalNotifications;

namespace MyNotes.NotePages
{
    delegate Task<List<Note>> GetTodayNotes();

    public partial class NotesPage : ContentPage
    {
        static Dictionary<string, GetTodayNotes> days = new Dictionary<string, GetTodayNotes>()
        {
            { "Monday", () => Monday.Database.GetNotesAsync()},
            { "Tuesday", () => Tuesday.Database.GetNotesAsync()},
            { "Wednesday", () => Wednesday.Database.GetNotesAsync()},
            { "Thursday", () => Thursday.Database.GetNotesAsync()},
            { "Friday", () => Friday.Database.GetNotesAsync()},
            { "Saturday", () => Saturday.Database.GetNotesAsync()},
            { "Sunday", () => Sunday.Database.GetNotesAsync()}
        };

        public NotesPage()
        {
            InitializeComponent();
            todayLabel.Text = DateTime.Now.DayOfWeek.ToString();
        }

        /// <summary>
        /// Метод генерации уведомлений на устройстве
        /// </summary>
        /// <param name="day">День напоминания</param>
        public static async void CreateSystemNotifications(string day)
        {
            List<Note> todayEnabledNotes;
            List<Note> todayDisabledNotes;
            string today = DateTime.Now.DayOfWeek.ToString();
            if (today == day)
            {
                var notes = await days[today]();
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
                    DateTime notificationTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                        DateTime.Now.Day, note.NotificationTime.Hours, note.NotificationTime.Minutes, 0);
                    // Проверка времени.
                    if (notificationTime > DateTime.Now)
                        CrossLocalNotifications.Current.Show("Уведомление MyNotes", 
                            $"{note.FormatTime} {note.NoteText}",
                            note.ID, notificationTime);
                }

                // Удаление уведомлений.
                foreach(var note in todayDisabledNotes)
                {
                    CrossLocalNotifications.Current.Cancel(note.ID);
                }
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