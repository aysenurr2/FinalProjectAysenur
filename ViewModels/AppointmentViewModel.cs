using FinalProjectAysenur.Models;
using FinalProjectAysenur.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FinalProjectAysenur.ViewModels
{
    public class AppointmentViewModel : BaseViewModel
    {
        private readonly DatabaseService _dbService;

        public ObservableCollection<Appointment> Appointments { get; set; } = new();
        public ObservableCollection<Pet> Pets { get; set; } = new();

        public AppointmentViewModel(DatabaseService dbService)
        {
            _dbService = dbService;
            Title = "Randevu Takip";
            
            // Commands
            LoadAppointmentsCommand = new Command(async () => await ExecuteLoadAppointments());
            SaveAppointmentCommand = new Command(async () => await ExecuteSaveAppointment());
            CancelAppointmentCommand = new Command<Appointment>(async (a) => await ExecuteCancelAppointment(a));
            GoToTodayCommand = new Command(() => SelectedDate = DateTime.Today);
            
            // Default Date
            SelectedDate = DateTime.Today;
        }

        public ICommand LoadAppointmentsCommand { get; }
        public ICommand SaveAppointmentCommand { get; }
        public ICommand CancelAppointmentCommand { get; }
        public ICommand GoToTodayCommand { get; }

        // --- Filter ---
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                SetProperty(ref _selectedDate, value);
                // When date changes, reload list
                if(LoadAppointmentsCommand != null) 
                   ExecuteLoadAppointments().FireAndForgetSafeAsync();
            }
        }

        // --- Create New ---
        private Pet _selectedPet;
        public Pet SelectedPet { get => _selectedPet; set => SetProperty(ref _selectedPet, value); }
        
        private TimeSpan _selectedTime = DateTime.Now.TimeOfDay;
        public TimeSpan SelectedTime { get => _selectedTime; set => SetProperty(ref _selectedTime, value); }

        private string _reason;
        public string Reason { get => _reason; set => SetProperty(ref _reason, value); }

        // --- Logic ---
        private async Task ExecuteLoadAppointments()
        {
            IsBusy = true;
            try
            {
                // 1. Load Pets for Helper Name
                var petList = await _dbService.GetPetsAsync();
                Pets.Clear();
                foreach (var p in petList) Pets.Add(p);

                // 2. Load Appointments
                var allAppointments = await _dbService.GetAppointmentsAsync();
                
                // Filter by Selected Date (Ignore Time part for filtering date)
                var filtered = allAppointments
                                .Where(a => a.AppointmentDate.Date == SelectedDate.Date)
                                .OrderBy(a => a.AppointmentDate) // Sort by time
                                .ToList();

                Appointments.Clear();
                foreach (var app in filtered)
                {
                    // Map Helper Helpers
                    var p = Pets.FirstOrDefault(x => x.Id == app.PetId);
                    app.PetName = p != null ? p.Name : "Silinmiş Hasta";
                    
                    Appointments.Add(app);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ExecuteSaveAppointment()
        {
            if (SelectedPet == null)
            {
                await Application.Current.MainPage.DisplayAlert("Hata", "Lütfen bir hasta seçiniz.", "Tamam");
                return;
            }

            // Construct Full Date
            var fullDate = SelectedDate.Date + SelectedTime;

            // Duplicate Check
            var allAppointments = await _dbService.GetAppointmentsAsync();
            bool isDuplicate = allAppointments.Any(a => !a.IsCancelled && a.AppointmentDate == fullDate);

            if (isDuplicate)
            {
                await Application.Current.MainPage.DisplayAlert("Uyarı", "Bu saatte zaten bir randevu mevcut!", "Tamam");
                return;
            }

            var newApp = new Appointment
            {
                PetId = SelectedPet.Id,
                AppointmentDate = fullDate,
                Reason = Reason,
                IsCancelled = false
            };

            await _dbService.SaveAppointmentAsync(newApp);

            // Clean Form
            SelectedPet = null;
            Reason = string.Empty;
            
            await ExecuteLoadAppointments();
            await Application.Current.MainPage.DisplayAlert("Başarılı", "Randevu oluşturuldu.", "Tamam");
        }

        private async Task ExecuteCancelAppointment(Appointment app)
        {
            if(app == null) return;

            bool answer = await Application.Current.MainPage.DisplayAlert("İptal", "Bu randevuyu iptal etmek istiyor musunuz?", "Evet", "Hayır");
            if (!answer) return;

            app.IsCancelled = true;
            await _dbService.SaveAppointmentAsync(app); // Update
            await ExecuteLoadAppointments();
        }
    }

    public static class TaskExtensions
    {
        public static async void FireAndForgetSafeAsync(this Task task, IErrorHandler handler = null)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                handler?.HandleError(ex);
            }
        }
    }
    
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}
