using System;
using Android.App;
using Android.Content;
using Android.OS;
using Xamarin.Forms;

namespace MyNotes.Droid
{
    [Service(Label = "MyNotesService")]
    public class MyNotesService : Service
    {
        Handler handler;
        Action runnable;

        public override void OnCreate()
        {
            base.OnCreate();
            // Определяем обработчик действия.
            handler = new Handler();
            // Задаем действие.
            runnable = new Action(() =>
            {
                Intent intent = new Intent("MyNotes.Notification.Action");
                intent.PutExtra("broadcast_message", "MyNotesAction");
                Android.Support.V4.Content.LocalBroadcastManager.GetInstance(this).SendBroadcast(intent);

            });
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            if (intent.Action.Equals("MyNotes.action.START_SERVICE"))
            {
                Device.StartTimer(TimeSpan.FromHours(6), () =>
                {
                    MessagingCenter.Send<string>(DateTime.Now.DayOfWeek.ToString(), "CreateSystemNotifications");
                    return true;
                });
                RegisterForegroundService();
            }
            else if (intent.Action.Equals("MyNotes.action.STOP_SERVICE"))
            {
                StopForeground(true);
                StopSelf();
            }

            return StartCommandResult.Sticky;
        }


        public override IBinder OnBind(Intent intent)
        {
            return null;
        }


        public override void OnDestroy()
        {
            // Завершение действия сервиса.
            handler.RemoveCallbacks(runnable);

            // Удаление уведомления о работе сервиса.
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Cancel(10000);
            base.OnDestroy();
        }

        /// <summary>
        /// Регистрация и запуск сервиса
        /// </summary>
        void RegisterForegroundService()
        {
            Notification.Builder notification;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                // Создаем канал уведомления.
                var channel = new NotificationChannel("location_notification", "Channel", NotificationImportance.Default)
                {
                    Description = "Foreground Service Channel"
                };

                var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);

                notification = new Notification.Builder(this, "location_notification");
            }
            else
            {
                notification = new Notification.Builder(this);
            }
            var notify = notification.SetContentTitle(Resources.GetString(Resource.String.app_name))
                                     .SetContentText(Resources.GetString(Resource.String.notification_text))
                                     .SetSmallIcon(Resource.Drawable.notification)
                                     .SetOngoing(true)
                                     .AddAction(BuildStopServiceAction())
                                     .Build();
            StartForeground(10000, notify);
        }

        /// <summary>
        /// Запускает действие, которое позволяет завершить работу сервиса
        /// </summary>
        /// <returns>Действие завершения сервиса</returns>
        Notification.Action BuildStopServiceAction()
        {
            var stopServiceIntent = new Intent(this, GetType());
            stopServiceIntent.SetAction("MyNotes.action.STOP_SERVICE");
            var stopServicePendingIntent = PendingIntent.GetService(this, 0, stopServiceIntent, 0);

            var builder = new Notification.Action.Builder(Android.Resource.Drawable.IcMediaPause,
                                                          GetText(Resource.String.stop_service),
                                                          stopServicePendingIntent);
            return builder.Build();

        }
    }
}
