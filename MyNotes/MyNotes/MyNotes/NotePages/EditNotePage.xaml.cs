using System;
using MyNotes.Models;
using Plugin.LocalNotifications;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyNotes.NotePages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditNotePage : ContentPage
    {
        public delegate void NoteHandler();
        // Событие при изменении заметок.
        public static event NoteHandler EditNoteEvent;

        NotesDatabase database;
        Note newNote;

		public EditNotePage(ref Note note, NotesDatabase db)
		{
            InitializeComponent();
            noteText.Text = note.NoteText;
            notificationTime.Time = note.NotificationTime;
            notify.IsChecked = note.IsNotify;
            database = db;
            newNote = note;
        }

        /// <summary>
        /// Метод сохранения изменений заметки
        /// </summary>
        /// <param name="sender">Отправитель Button</param>
        /// <param name="e">Событие отправителя</param>
        async void EditNoteClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(noteText.Text))
            {
                newNote.NoteText = noteText.Text;
                newNote.NotificationTime = notificationTime.Time;
                newNote.IsNotify = notify.IsChecked;
                if (!newNote.IsNotify) CrossLocalNotifications.Current.Cancel(newNote.ID);
                // Сохранение измененной заметки.
                await database.SaveNoteAsync(newNote);

                // Подписываем метод на событие.
                var page = Navigation.NavigationStack[0] as NotesPage;
                EditNoteEvent += page.UpdateNowNotes;
                // Вызываем событие.
                EditNoteEvent?.Invoke();
                DependencyService.Get<IMessage>().ShortAlert("Заметка успешно изменена");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Предупреждение!", "Нет текста заметки!!!", "ОК");
            }
        }
    }
}