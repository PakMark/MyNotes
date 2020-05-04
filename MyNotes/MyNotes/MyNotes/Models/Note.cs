using System;
using SQLite;

namespace MyNotes.Models
{
    /// <summary>
    /// Класс заметки
    /// </summary>
    public class Note
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string NoteText { get; set; }
        public TimeSpan NotificationTime { get; set; }
        public bool IsNotify { get; set; }

        public string FormatTime => $"{NotificationTime.ToString(@"hh\:mm")}";
    }
}
