using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace MyNotes.Models
{
    /// <summary>
    /// База данных заметок
    /// </summary>
    public class NotesDatabase
    {
        SQLiteAsyncConnection database;

        /// <summary>
        /// Создание базы данных
        /// </summary>
        /// <param name="dbPath">Путь к базе данных</param>
        public NotesDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Note>().Wait();
        }

        /// <summary>
        /// Метод получения содержимого базы данных
        /// </summary>
        /// <returns>Содержимое базы данных</returns>
        public Task<List<Note>> GetNotificationAsync()
        {
            return database.Table<Note>().ToListAsync();
        }

        /// <summary>
        /// Метод сохранения заметок в базе данных
        /// </summary>
        /// <param name="notes">Заметка</param>
        /// <returns>База данных с сохраненной заметкой</returns>
        public Task<int> SaveNotificationAsync(Note notes)
        {
            // Обновляем, если такое напоминание уже есть.
            if (notes.ID != 0)
                return database.UpdateAsync(notes);
            else
                return database.InsertAsync(notes);
        }

        /// <summary>
        /// Удаление заметки из базы даных
        /// </summary>
        /// <param name="id">ID заметки</param>
        /// <returns>База данных с удаленной заметкой</returns>
        public Task<int> DeleteNotification(int id)
        {
            return database.DeleteAsync<Note>(id);
        }
    }
}
