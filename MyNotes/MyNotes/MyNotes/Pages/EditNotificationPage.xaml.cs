using System;
using MyNotes.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyNotes.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditNotificationPage : ContentPage
    {
        public delegate void NotificationHandler();
        // Событие при изменении напоминания.
        public event NotificationHandler EditNotificationEvent;

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

        async void EditNotificationClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(notificationText.Text))
            {
                oldNotification.NotificationText = notificationText.Text;
                oldNotification.NotificationTime = notificationTime.Time;
                oldNotification.IsNotify = notify.IsChecked;
                // Сохранение измененного напоминания.
                await database.SaveNotificationAsync(oldNotification);
                // Подписываем метод на событие.
                var page = Navigation.NavigationStack[0] as MainPage;
                EditNotificationEvent += page.UpdateNowNotifications;
                // Вызываем событие.
                EditNotificationEvent?.Invoke();
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