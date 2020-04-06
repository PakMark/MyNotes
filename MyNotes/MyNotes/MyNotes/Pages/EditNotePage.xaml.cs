using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyNotes.Models;

namespace MyNotes.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditNotePage : ContentPage
	{
        NotesDatabase database;
        Note oldNote;

        public EditNotePage(ref Note note, NotesDatabase db)
        {
            InitializeComponent();
            noteEditor.Text = note.NoteText;
            database = db;
            oldNote = note;
        }

        /// <summary>
        /// Метод сохранения изменений заметки
        /// </summary>
        /// <param name="sender">Отправитель Button</param>
        /// <param name="e">Событие отправителя</param>
        async void EditNoteClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(noteEditor.Text))
            {
                oldNote.NoteText = noteEditor.Text;
                // Сохранение измененной заметки.
                await database.SaveNotificationAsync(oldNote);
                
                await DisplayAlert("Уведомление", "Заметка успешно изменена", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Предупреждение!", "Нет текста заметки!!!", "ОК");
            }
        }
    }
}