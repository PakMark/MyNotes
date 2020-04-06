using System;
using SQLite;

namespace MyNotes.Models
{
    /// <summary>
    /// Класс напоминания
    /// </summary>
    public class Notification
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string NotificationText { get; set; }
        public TimeSpan NotificationTime { get; set; }
        public bool IsNotify { get; set; }

        public string FormatTime => $"{NotificationTime.ToString(@"hh\:mm")}";
    }
}
