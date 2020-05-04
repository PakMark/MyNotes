using SQLite;

namespace MyNotes.Models
{
    /// <summary>
    /// Класс записи
    /// </summary>
    public class Memory
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string MemoryText { get; set; }
    }
}
