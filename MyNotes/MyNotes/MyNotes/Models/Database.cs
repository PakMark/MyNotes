using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace MyNotes.Models
{
    /// <summary>
    /// База данных напоминаний
    /// </summary>
    public class Database
    {
        SQLiteAsyncConnection database;

        /// <summary>
        /// Создание базы данных
        /// </summary>
        /// <param name="dbPath">Путь к базе данных</param>
        public Database(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Notification>().Wait();
        }

        /// <summary>
        /// Метод получения содержимого базы данных
        /// </summary>
        /// <returns>Содержимое базы данных</returns>
        public Task<List<Notification>> GetNotificationAsync()
        {
            return database.Table<Notification>().ToListAsync();
        }

        /// <summary>
        /// Метод сохранения напоминания в базе данных
        /// </summary>
        /// <param name="notification">Напоминание</param>
        /// <returns>База данных с сохраненным напоминанием</returns>
        public Task<int> SaveNotificationAsync(Notification notification)
        {
            // Обновляем, если такое напоминание уже есть.
            if (notification.ID != 0)
                return database.UpdateAsync(notification);
            else
                return database.InsertAsync(notification);
        }

        /// <summary>
        /// Удаление напоминания из базы даных
        /// </summary>
        /// <param name="id">ID напоминания</param>
        /// <returns>База данных с удаленным напоминанием</returns>
        public Task<int> DeleteNotification(int id)
        {
            return database.DeleteAsync<Notification>(id);
        }

        /// <summary>
        /// Метод получения количества элементов в базе данных
        /// </summary>
        /// <returns>Число элементов в базе данных</returns>
        public Task<int> GetQuantityNotifications()
        {
            return database.Table<Notification>().CountAsync();
        }
    }
}
