using SQLite;
using FinalProjectAysenur.Models;

namespace FinalProjectAysenur.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        async Task Init()
        {
            if (_database is not null) return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "VetDb.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            await _database.CreateTableAsync<Pet>();
        }

        public async Task<List<Pet>> GetPetsAsync()
        {
            await Init();
            return await _database.Table<Pet>().ToListAsync();
        }

        public async Task<int> SavePetAsync(Pet pet)
        {
            await Init();
            return await _database.InsertAsync(pet);
        }

            
        public async Task<int> DeletePetAsync(Pet pet) 

        {
            await Init();
            return await _database.DeleteAsync(pet);
        }
    }
}