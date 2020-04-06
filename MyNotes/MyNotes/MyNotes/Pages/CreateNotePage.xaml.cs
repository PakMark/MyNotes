using System;
using MyNotes.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyNotes.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateNotePage : ContentPage
	{
		public CreateNotePage ()
		{
			InitializeComponent ();
		}

        /// <summary>
        /// Обработчик кнопки "Создать заметку"
        /// </summary>
        /// <param name="sender">Отправитель Button</param>
        /// <param name="e">Событие</param>
        async void CreateNoteClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(noteEditor.Text))
            {
                // Сохранение напоминания в базе данных.
                await NotesPage.Database.SaveNotificationAsync(new Note
                {
                    NoteText = noteEditor.Text
                });
                noteEditor.Text = string.Empty;
                await DisplayAlert("Уведомление", "Заметка успешно сохранена", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Предупреждение!", "Нет текста заметки!!!", "ОК");
            }
        }
    }
}