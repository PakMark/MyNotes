using System;
using System.Linq;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyNotes.Models;

namespace MyNotes.Days
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Wednesday : ContentPage
    {
        static Database database;
        /// <summary>
        /// Создание базы данных
        /// </summary>
        public static Database Database
        {
            get
            {
                if (database == null)
                {
                    database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder
                        .LocalApplicationData), "WednesdayNotifications.db3"));
                }
                return database;
            }
        }

        /// <summary>
        /// Метод проверки базы данных на пустоту
        /// </summary>
        /// <returns>Индикатор пустоты</returns>
        static bool IsDatabaseEmpty()
        {
            int quantity = database.GetQuantityNotifications().Result;
            if (quantity == 0) return true;
            return false;
        }

        /// <summary>
        /// Конструктор страницы
        /// </summary>
        public Wednesday()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Действия при загрузке страницы
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // Загрузка базы данных
            var notifications = await Wednesday.Database.GetNotificationAsync();
            // Уведомление напоминаний.
            Pages.NotificationsPage.CreateSystemNotifications("Wednesday");

            // Сортировка напоминаний по времени и передеча в ListView.
            listView.ItemsSource = notifications.OrderBy(X => X.NotificationTime)
                                                      .OrderByDescending(X => X.IsNotify);
        }

        /// <summary>
        /// Обработчик кнопки "Добавить напоминание"
        /// </summary>
        /// <param name="sender">Отправитель Button</param>
        /// <param name="e">Событие</param>
        async void AddNotificationClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(notificationText.Text))
            {
                // Сохранение напоминания в базе данных.
                await Wednesday.Database.SaveNotificationAsync(new Notification
                {
                    NotificationText = notificationText.Text,
                    NotificationTime = notificationTime.Time,
                    IsNotify = notify.IsChecked
                });
                notificationText.Text = string.Empty;
                OnAppearing();
            }
            else
            {
                await DisplayAlert("Предупреждение!", "Нет текста напоминания!!!", "ОК");
            }
        }

        /// <summary>
        /// Метод удаления напоминания.
        /// </summary>
        /// <param name="sender">Отправитель SwipeView</param>
        /// <param name="e">Событие</param>
        async void DeleteCommand(object sender, EventArgs e)
        {
            // Обработка выбранного объекта SwipeView.
            MenuItem menuItem = sender as MenuItem;
            var notification = (Notification)menuItem.BindingContext;
            // Удаление напоминания.
            await database.DeleteNotification(notification.ID);
            OnAppearing();
        }

        /// <summary>
        /// Метод изменения напоминания
        /// </summary>
        /// <param name="sender">Отправитель SwipeView</param>
        /// <param name="e">Событие</param>
        async void EditCommand(object sender, EventArgs e)
        {
            // Обработка выбранного объекта SwipeView.
            MenuItem menuItem = sender as MenuItem;
            var notification = (Notification)menuItem.BindingContext;
            // Вызов новой страницы для изменения напоминания.
            await Navigation.PushAsync(new Pages.EditNotificationPage(ref notification, Database));
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
            Notification notification = (Notification)sw.BindingContext;
            if (notification != null)
            {
                await database.SaveNotificationAsync(notification);
                base.OnAppearing();
            }
        }
    }
}