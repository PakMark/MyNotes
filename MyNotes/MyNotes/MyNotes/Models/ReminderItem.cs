using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyNotes.Models
{
    public class ReminderItem : INotifyPropertyChanged
    {
        //[PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }

        public string VehicleType { get; set; }

        public bool IsReminderEnabled { get; set; }

        public DateTime NextReminder { get; set; }

        public DateTime LastServiceDate { get; set; }

        public DateTime NextServiceDate { get; set; }

        public string Notes { get; set; }



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
