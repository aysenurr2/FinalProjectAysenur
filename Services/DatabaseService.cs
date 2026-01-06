using SQLite;
using FinalProjectAysenur.Models;

namespace FinalProjectAysenur.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        // Veritabanı başlatma (Tabloları oluşturma)
        // Bu metot her veritabanı işleminden önce çağrılır, bağlantı yoksa açar.
        async Task Init()
        {
            if (_database is not null) return;

            // Veritabanı dosyasının yolu (Cihazın yerel uygulama klasörü)
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "VetDb.db3");
            _database = new SQLiteAsyncConnection(dbPath);

            // Tabloları oluştur
            await _database.CreateTableAsync<Clinic>();
            await _database.CreateTableAsync<Pet>();
            await _database.CreateTableAsync<Owner>();
            await _database.CreateTableAsync<Appointment>();
            await _database.CreateTableAsync<Treatment>(); 
            await _database.CreateTableAsync<Finance>(); 
        }

        // --- Clinic / Login ---
        public async Task<Clinic> LoginOrRegisterAsync(string email, string password)
        {
            await Init();
            var existingClinic = await _database.Table<Clinic>()
                                                 .Where(u => u.Email == email)
                                                 .FirstOrDefaultAsync();

            if (existingClinic != null) return existingClinic;
            else
            {
                var newClinic = new Clinic
                {
                    Email = email,
                    Password = password,
                    ClinicName = "Veteriner Kliniği"
                };
                await _database.InsertAsync(newClinic);
                return newClinic;
            }
        }

        // --- Pet (Soft Delete & Filter) ---
        public async Task<List<Pet>> GetPetsAsync()
        {
            await Init();
            return await _database.Table<Pet>().Where(p => p.IsActive).ToListAsync();
        }

        public async Task<int> SavePetAsync(Pet pet)
        {
            await Init();
            if (pet.Id != 0) return await _database.UpdateAsync(pet);
            else return await _database.InsertAsync(pet);
        }

        public async Task<int> DeletePetAsync(Pet pet)
        {
            await Init();
            pet.IsActive = false; // Soft Delete
            return await _database.UpdateAsync(pet);
        }

        // --- Owner (Soft Delete & Filter) ---
        public async Task<List<Owner>> GetOwnersAsync()
        {
            await Init();
            return await _database.Table<Owner>().Where(o => o.IsActive).ToListAsync();
        }

        public async Task<int> SaveOwnerAsync(Owner owner)
        {
            await Init();
            if (owner.Id != 0) return await _database.UpdateAsync(owner);
            else return await _database.InsertAsync(owner);
        }

        public async Task<int> DeleteOwnerAsync(Owner owner)
        {
            await Init();
            owner.IsActive = false; // Soft Delete
            return await _database.UpdateAsync(owner);
        }

        // --- Appointment ---
        public async Task<List<Appointment>> GetAppointmentsAsync()
        {
            await Init();
            return await _database.Table<Appointment>().ToListAsync();
        }

        public async Task<int> SaveAppointmentAsync(Appointment appointment)
        {
            await Init();
            if (appointment.Id != 0) return await _database.UpdateAsync(appointment);
            else return await _database.InsertAsync(appointment);
        }

        public async Task<int> DeleteAppointmentAsync(Appointment appointment)
        {
            await Init();
            return await _database.DeleteAsync(appointment);
        }

        // --- Treatment & Finance Logic ---
        
        // Save Treatment and automatically add Finance record
        public async Task ProcessTreatmentAsync(Treatment treatment)
        {
            await Init();
            
            
            if(treatment.Id != 0) await _database.UpdateAsync(treatment);
            else await _database.InsertAsync(treatment);

            // Finance Record
            var finance = new Finance
            {
                TreatmentId = treatment.Id,
                Date = treatment.Date,
                Amount = treatment.TotalAmount,
            };
            await _database.InsertAsync(finance);
        }

        // Finance List (Read Only)
        public async Task<List<Finance>> GetFinancesAsync()
        {
             await Init();
             return await _database.Table<Finance>().OrderByDescending(f => f.Date).ToListAsync();
        }

        // Just in case we need to list treatments
        public async Task<List<Treatment>> GetTreatmentsAsync()
        {
            await Init();
            return await _database.Table<Treatment>().OrderByDescending(t => t.Date).ToListAsync();
        }
    }
}