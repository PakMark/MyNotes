using System;
using System.IO;
using MyNotes.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyNotes.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NotesPage : ContentPage
	{
		public NotesPage ()
		{
			InitializeComponent ();
		}

        static NotesDatabase database;
        /// <summary>
        /// Создание базы данных
        /// </summary>
        public static NotesDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new NotesDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder
                        .LocalApplicationData), "Notes.db3"));
                }
                return database;
            }
        }

        /// <summary>
        /// Действия при загрузке страницы
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // Загрузка базы данных
            var notes = await Database.GetNotificationAsync();
            listView.ItemsSource = notes;
        }

        /// <summary>
        /// Обработчик кнопки "Добавить заметку"
        /// </summary>
        /// <param name="sender">Отправитель Button</param>
        /// <param name="e">Событие</param>
        async void AddNoteClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateNotePage());
            OnAppearing();
        }

        /// <summary>
        /// Метод удаления заметки.
        /// </summary>
        /// <param name="sender">Отправитель SwipeView</param>
        /// <param name="e">Событие</param>
        async void DeleteCommand(object sender, EventArgs e)
        {
            // Обработка выбранного объекта SwipeView.
            MenuItem menuItem = sender as MenuItem;
            var note = (Note)menuItem.BindingContext;
            // Удаление напоминания.
            await database.DeleteNotification(note.ID);
            OnAppearing();
        }

        /// <summary>
        /// Метод изменения заметки
        /// </summary>
        /// <param name="sender">Отправитель SwipeView</param>
        /// <param name="e">Событие</param>
        async void EditCommand(object sender, EventArgs e)
        {
            // Обработка выбранного объекта SwipeView.
            MenuItem menuItem = sender as MenuItem;
            var note = (Note)menuItem.BindingContext;
            // Вызов новой страницы для изменения напоминания.
            await Navigation.PushAsync(new Pages.EditNotePage(ref note, Database));
            OnAppearing();
        }
    }
}