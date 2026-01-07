using System.Collections.ObjectModel;
using System.Windows.Input;
using FinalProjectAysenur.Models;
using FinalProjectAysenur.Services;

namespace FinalProjectAysenur.ViewModels
{
    public class TreatmentViewModel : BaseViewModel
    {
        private readonly DatabaseService _dbService;

        public ObservableCollection<Pet> Pets { get; set; } = new();
        public ObservableCollection<ServiceItem> AvailableServices { get; set; } = new();

        public TreatmentViewModel(DatabaseService dbService)
        {
            _dbService = dbService;
            Title = "Yeni Tedavi";

            LoadDataCommand = new Command(async () => await LoadDataAsync());
            SaveTreatmentCommand = new Command(async () => await SaveTreatmentAsync());
            ToggleServiceCommand = new Command<ServiceItem>((item) => ToggleService(item));

            InitializeServices();
        }

        public ICommand LoadDataCommand { get; }
        public ICommand SaveTreatmentCommand { get; }
        public ICommand ToggleServiceCommand { get; }

        private Pet _selectedPet;
        public Pet SelectedPet { get => _selectedPet; set => SetProperty(ref _selectedPet, value); }

        private DateTime _date = DateTime.Now;
        public DateTime Date { get => _date; set => SetProperty(ref _date, value); }

        private string _description;
        public string Description { get => _description; set => SetProperty(ref _description, value); }

        private decimal _totalAmount;
        public decimal TotalAmount { get => _totalAmount; set => SetProperty(ref _totalAmount, value); }
        
        private void InitializeServices()
        {
            AvailableServices.Add(new ServiceItem { Name = "Genel Muayene", Price = 200, Category = "Muayene" });
            AvailableServices.Add(new ServiceItem { Name = "Kontrol Muayenesi", Price = 150, Category = "Muayene" });
            
            AvailableServices.Add(new ServiceItem { Name = "Karma Aşı", Price = 300, Category = "Aşı" });
            AvailableServices.Add(new ServiceItem { Name = "Kuduz Aşısı", Price = 250, Category = "Aşı" });
            AvailableServices.Add(new ServiceItem { Name = "İç Parazit", Price = 200, Category = "Aşı" });
            AvailableServices.Add(new ServiceItem { Name = "Dış Parazit", Price = 200, Category = "Aşı" });
            
            AvailableServices.Add(new ServiceItem { Name = "Serum Takılması", Price = 350, Category = "Müdahale" });
            AvailableServices.Add(new ServiceItem { Name = "Enjeksiyon", Price = 150, Category = "Müdahale" });
            AvailableServices.Add(new ServiceItem { Name = "Yara Tedavisi", Price = 400, Category = "Müdahale" });
            
            AvailableServices.Add(new ServiceItem { Name = "Kısırlaştırma", Price = 1500, Category = "Cerrahi" });
            AvailableServices.Add(new ServiceItem { Name = "Küçük Cerrahi İşlem", Price = 1000, Category = "Cerrahi" });
        }

        private async Task LoadDataAsync()
        {
            var pets = await _dbService.GetPetsAsync();
            Pets.Clear();
            foreach (var p in pets) Pets.Add(p);
        }

        private void ToggleService(ServiceItem item)
        {
            if (item == null) return;
            item.IsSelected = !item.IsSelected;
            
            var index = AvailableServices.IndexOf(item);
            if (index >= 0)
            {
                AvailableServices.RemoveAt(index);
                AvailableServices.Insert(index, item);
            }

            CalculateTotal();
        }

        public void CalculateTotal()
        {
            TotalAmount = AvailableServices.Where(s => s.IsSelected).Sum(s => s.Price);
        }

        private async Task SaveTreatmentAsync()
        {
            if (SelectedPet == null)
            {
                await Application.Current.MainPage.DisplayAlert("Hata", "Lütfen bir hasta seçiniz.", "Tamam");
                return;
            }

            var selectedServices = AvailableServices.Where(s => s.IsSelected).ToList();
            if (!selectedServices.Any())
            {
                await Application.Current.MainPage.DisplayAlert("Hata", "En az bir işlem seçmelisiniz.", "Tamam");
                return;
            }

            string serviceListStr = string.Join(", ", selectedServices.Select(s => s.Name));
            
            var treatment = new Treatment
            {
                PetId = SelectedPet.Id,
                Date = Date,
                ServiceList = serviceListStr,
                TotalAmount = TotalAmount,
                Description = Description
            };

            await _dbService.ProcessTreatmentAsync(treatment);
            
            SelectedPet = null;
            Description = string.Empty;
            Date = DateTime.Now;
            foreach (var s in AvailableServices) s.IsSelected = false;
            
            var temp = new List<ServiceItem>(AvailableServices);
            AvailableServices.Clear();
            foreach(var t in temp) AvailableServices.Add(t);

            CalculateTotal();
            await Application.Current.MainPage.DisplayAlert("Başarılı", "Tedavi kaydedildi.", "Tamam");
        }
    }
}
