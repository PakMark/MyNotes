using System;
using MyNotes.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyNotes.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditNotificationPage : ContentPage
    {
        Database database;
        Notification oldNotification;

		public EditNotificationPage (ref Notification notification, Database db)
		{
            InitializeComponent();
            notificationText.Text = notification.NotificationText;
            notificationTime.Time = notification.NotificationTime;
            notify.IsChecked = notification.IsNotify;
            database = db;
            oldNotification = notification;
        }

        /// <summary>
        /// Метод сохранения изменений напоминания
        /// </summary>
        /// <param name="sender">Отправитель Button</param>
        /// <param name="e">Событие отправителя</param>
        async void EditNotificationClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(notificationText.Text))
            {
                oldNotification.NotificationText = notificationText.Text;
                oldNotification.NotificationTime = notificationTime.Time;
                oldNotification.IsNotify = notify.IsChecked;
                // Сохранение измененного напоминания.
                await database.SaveNotificationAsync(oldNotification);

                await DisplayAlert("Уведомление", "Напоминание успешно изменено", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Предупреждение!", "Нет текста напоминания!!!", "ОК");
            }
        }
    }
}