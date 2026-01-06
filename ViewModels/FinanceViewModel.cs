using System.Collections.ObjectModel;
using System.Windows.Input;
using FinalProjectAysenur.Models;
using FinalProjectAysenur.Services;

namespace FinalProjectAysenur.ViewModels
{
    public class FinanceViewModel : BaseViewModel
    {
        private readonly DatabaseService _dbService;
        public ObservableCollection<Finance> Finances { get; set; } = new();

        public FinanceViewModel(DatabaseService dbService)
        {
            _dbService = dbService;
            Title = "Kasa & Finans";
            LoadDataCommand = new Command(async () => await LoadDataAsync());
        }

        public ICommand LoadDataCommand { get; }

        private decimal _totalIncome;
        public decimal TotalIncome { get => _totalIncome; set => SetProperty(ref _totalIncome, value); }

        private async Task LoadDataAsync()
        {
            IsBusy = true;
            try
            {
                var finances = await _dbService.GetFinancesAsync();
                var treatments = await _dbService.GetTreatmentsAsync(); // Fetch treatments
                var pets = await _dbService.GetPetsAsync();
                var owners = await _dbService.GetOwnersAsync();

                Finances.Clear();
                decimal total = 0;
                
                foreach (var f in finances)
                {
                    // Find related treatment
                    var treatment = treatments.FirstOrDefault(t => t.Id == f.TreatmentId);
                    if (treatment != null)
                    {
                        var pet = pets.FirstOrDefault(p => p.Id == treatment.PetId);
                        if (pet != null)
                        {
                            f.PetName = pet.Name;
                            var owner = owners.FirstOrDefault(o => o.Id == pet.OwnerId);
                            f.OwnerName = owner != null ? owner.FullName : "-";
                        }
                        else
                        {
                             f.PetName = "Bilinmiyor";
                             f.OwnerName = "-";
                        }
                    }
                    else
                    {
                        f.PetName = "Silinmiş İşlem";
                        f.OwnerName = "-";
                    }

                    // In simplified version, we assume everything is valid income
                    total += f.Amount;
                    Finances.Add(f);
                }
                TotalIncome = total;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
