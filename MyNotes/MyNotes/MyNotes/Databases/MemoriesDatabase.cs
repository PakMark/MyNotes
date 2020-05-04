using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace MyNotes.Models
{
    /// <summary>
    /// База данных записей
    /// </summary>
    public class MemoriesDatabase
    {
        SQLiteAsyncConnection database;

        /// <summary>
        /// Создание базы данных
        /// </summary>
        /// <param name="dbPath">Путь к базе данных</param>
        public MemoriesDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Memory>().Wait();
        }

        /// <summary>
        /// Метод получения содержимого базы данных
        /// </summary>
        /// <returns>Содержимое базы данных</returns>
        public Task<List<Memory>> GetMemoriesAsync()
        { 
            return database.Table<Memory>().ToListAsync();
        }

        /// <summary>
        /// Метод сохранения записи в базе данных
        /// </summary>
        /// <param name="memory">Запись</param>
        /// <returns>База данных с сохраненной записью</returns>
        public Task<int> SaveMemoryAsync(Memory memory)
        {
            // Обновляем, если такая запись уже есть.
            if (memory.ID != 0)
                return database.UpdateAsync(memory);
            else
                return database.InsertAsync(memory);
        }

        /// <summary>
        /// Удаление записи из базы даных
        /// </summary>
        /// <param name="id">ID записи</param>
        /// <returns>База данных с удаленной записью</returns>
        public Task<int> DeleteMemoryAsync(int id)
        {
            return database.DeleteAsync<Memory>(id);
        }
    }
}
