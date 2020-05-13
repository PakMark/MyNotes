using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using MyNotes.Models;
using MyNotes.Days;

namespace MyNotes.NotePages
{
    delegate Task<List<Note>> GetTodayNotes();

    public partial class NotesPage : ContentPage
    {
        internal static Dictionary<string, GetTodayNotes> days = new Dictionary<string, GetTodayNotes>()
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
            // Загружаем сегодняшние заметки.
            UpdateNowNotes();
        }

        /// <summary>
        /// Метод повторного обновления сегодняшних заметок
        /// </summary>
        public async void UpdateNowNotes()
        {
            string today = DateTime.Now.DayOfWeek.ToString();
            todayLabel.Text = today;
            var notes = await days[today]();
            // Получение сегодняшних заметок и передеча в listView.
            var todayNotes = notes.OrderBy(x => x.NotificationTime)
                                  .OrderByDescending(x => x.IsNotify)
                                  .TakeWhile(x => x.IsNotify == true)
                                  .ToList();
            listView.ItemsSource = todayNotes;
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
    }
}