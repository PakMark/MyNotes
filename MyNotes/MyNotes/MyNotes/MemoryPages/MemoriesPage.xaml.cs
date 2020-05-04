using System;
using System.IO;
using MyNotes.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyNotes.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MemoriesPage : ContentPage
    {
        public MemoriesPage()
        {
            InitializeComponent();
        }

        static MemoriesDatabase database;
        /// <summary>
        /// Создание базы данных
        /// </summary>
        public static MemoriesDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new MemoriesDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder
                        .LocalApplicationData), "Memories.db3"));
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
            // Загрузка базы данных.
            var memories = await Database.GetMemoriesAsync();
            listView.ItemsSource = memories;
        }

        /// <summary>
        /// Обработчик кнопки "Добавить запись"
        /// </summary>
        /// <param name="sender">Отправитель Button</param>
        /// <param name="e">Событие</param>
        async void AddMemoryClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateMemoryPage());
            OnAppearing();
        }

        /// <summary>
        /// Метод удаления записи.
        /// </summary>
        /// <param name="sender">Отправитель SwipeView</param>
        /// <param name="e">Событие</param>
        async void DeleteCommand(object sender, EventArgs e)
        {
            // Обработка выбранного объекта SwipeView.
            MenuItem menuItem = sender as MenuItem;
            var memory = (Memory)menuItem.BindingContext;
            // Удаление записи.
            await database.DeleteMemoryAsync(memory.ID);
            OnAppearing();
        }

        /// <summary>
        /// Метод изменения записи
        /// </summary>
        /// <param name="sender">Отправитель SwipeView</param>
        /// <param name="e">Событие</param>
        async void EditCommand(object sender, EventArgs e)
        {
            // Обработка выбранного объекта SwipeView.
            MenuItem menuItem = sender as MenuItem;
            var memory = (Memory)menuItem.BindingContext;
            // Вызов новой страницы для изменения записи.
            await Navigation.PushAsync(new EditMemoryPage(ref memory, Database));
            OnAppearing();
        }
    }
}