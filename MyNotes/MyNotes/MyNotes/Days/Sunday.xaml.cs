using System;
using System.Linq;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyNotes.Models;
using Plugin.LocalNotifications;
using MyNotes.NotePages;

namespace MyNotes.Days
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Sunday : ContentPage
    {
        public delegate void NoteHandler();
        // Событие при изменении заметок.
        public static event NoteHandler EditNoteEvent;

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
                        .LocalApplicationData), "SundayNotes.db3"));
                }
                return database;
            }
        }

        /// <summary>
        /// Конструктор страницы
        /// </summary>
        public Sunday()
        {
            InitializeComponent();
            // Команда для обновления списка заметок.
            listView.RefreshCommand = new Command(() => {
                OnAppearing();
                listView.IsRefreshing = false;
            });
        }

        /// <summary>
        /// Действия при загрузке страницы
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // Загрузка базы данных.
            var notes = await Sunday.Database.GetNotesAsync();

            // Подписываем метод на событие.
            var page = Navigation.NavigationStack[0] as NotesPage;
            EditNoteEvent += page.UpdateNowNotes;
            // Вызываем событие.
            EditNoteEvent?.Invoke();

            // Подготовка уведомлений.
            NotesPage.CreateSystemNotifications("Sunday");

            // Сортировка заметок по времени и передеча в ListView.
            listView.ItemsSource = notes.OrderBy(X => X.NotificationTime)
                                        .OrderByDescending(X => X.IsNotify);
        }

        /// <summary>
        /// Обработчик кнопки "Добавить заметку"
        /// </summary>
        /// <param name="sender">Отправитель Button</param>
        /// <param name="e">Событие</param>
        async void AddNoteClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(noteText.Text))
            {
                // Сохранение заметки в базе данных.
                await Sunday.Database.SaveNoteAsync(new Note
                {
                    NoteText = noteText.Text,
                    NotificationTime = notificationTime.Time,
                    IsNotify = notify.IsChecked
                });
                noteText.Text = string.Empty;
                DependencyService.Get<IMessage>().ShortAlert("Заметка создана");
                OnAppearing();
            }
            else
            {
                await DisplayAlert("Предупреждение!", "Нет заметки!!!", "ОК");
            }
        }

        /// <summary>
        /// Команда удаления заметки.
        /// </summary>
        /// <param name="sender">Отправитель SwipeView</param>
        /// <param name="e">Событие</param>
        async void DeleteCommand(object sender, EventArgs e)
        {
            // Обработка выбранного объекта SwipeView.
            MenuItem menuItem = sender as MenuItem;
            var note = (Note)menuItem.BindingContext;
            CrossLocalNotifications.Current.Cancel(note.ID);
            // Удаление заметки.
            await database.DeleteNote(note.ID);
            DependencyService.Get<IMessage>().ShortAlert("Заметка удалена");
            OnAppearing();
        }

        /// <summary>
        /// Команда изменения заметки
        /// </summary>
        /// <param name="sender">Отправитель SwipeView</param>
        /// <param name="e">Событие</param>
        async void EditCommand(object sender, EventArgs e)
        {
            // Обработка выбранного объекта SwipeView.
            MenuItem menuItem = sender as MenuItem;
            var note = (Note)menuItem.BindingContext;
            // Вызов новой страницы для изменения заметки.
            await Navigation.PushAsync(new EditNotePage(ref note, Database));
            OnAppearing();
        }

        /// <summary>
        /// Метод изменения переключателя уведомления
        /// </summary>
        /// <param name="sender">Отправитель Switch</param>
        /// <param name="e">Событие</param>
        async void EditNotify(object sender, ToggledEventArgs e)
        {
            Switch sw = sender as Switch;
            Note note = (Note)sw.BindingContext;
            if (note != null)
            {
                if (!note.IsNotify) CrossLocalNotifications.Current.Cancel(note.ID);
                await database.SaveNoteAsync(note);
                base.OnAppearing();
                NotesPage.CreateSystemNotifications("Sunday");
            }
        }
    }
}