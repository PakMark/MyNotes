using System;
using System.Collections.Generic;
using System.Text;

namespace MyNotes.Models
{
    public interface IReminderService
    {
        void Remind(DateTime dateTime, string title, string message);
    }
}
