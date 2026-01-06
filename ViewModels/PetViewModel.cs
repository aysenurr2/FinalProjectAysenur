using System.Collections.ObjectModel;
using System.Windows.Input;
using FinalProjectAysenur.Models;
using FinalProjectAysenur.Services;

namespace FinalProjectAysenur.ViewModels
{
    public class PetViewModel : BaseViewModel
    {
        // Veritabanı işlemlerini yürüten servis
        private readonly DatabaseService _dbService;

        // ObservableCollection: UI tarafında listenin otomatik güncellenmesini sağlar.
        // Listeye eleman eklendiğinde veya çıkarıldığında arayüz (View) haberdar olur.
        public ObservableCollection<Pet> Pets { get; set; } = new();
        public ObservableCollection<Owner> Owners { get; set; } = new();

        // New Pet Fields - Bu property'ler View'daki Entry'lere bağlanır (Binding).
        // SetProperty metodu, değer değiştiğinde UI'ı uyarır (INotifyPropertyChanged).
        private string _name;    
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _species;
        public string Species
        {
            get => _species;
            set => SetProperty(ref _species, value);
        }

        private int _age;
        public int Age
        {
            get => _age;
            set => SetProperty(ref _age, value);
        }

        private string _symptom;
        public string Symptom
        {
            get => _symptom;
            set => SetProperty(ref _symptom, value);
        }

        private Owner _selectedOwner;
        public Owner SelectedOwner
        {
            get => _selectedOwner;
            set => SetProperty(ref _selectedOwner, value);
        }

        public ICommand LoadPetsCommand { get; }
        public ICommand AddPetCommand { get; }
        public ICommand DeletePetCommand { get; }
        public ICommand LoadOwnersCommand { get; }

        public PetViewModel(DatabaseService dbService)
        {
            _dbService = dbService;
            LoadPetsCommand = new Command(async () => await LoadPetsAsync());
            AddPetCommand = new Command(async () => await AddPetAsync());
            DeletePetCommand = new Command<Pet>(async (p) => await DeletePetAsync(p));
            LoadOwnersCommand = new Command(async () => await LoadOwnersAsync());
        }

        public async Task LoadPetsAsync()
        {
            var pets = await _dbService.GetPetsAsync();
            Pets.Clear();
            foreach (var pet in pets)
            {
                Pets.Add(pet);
            }
        }

        public async Task LoadOwnersAsync()
        {
            var owners = await _dbService.GetOwnersAsync();
            Owners.Clear();
            foreach (var owner in owners)
            {
                Owners.Add(owner);
            }
        }

        public async Task AddPetAsync()
        {
            if (string.IsNullOrWhiteSpace(Name) || SelectedOwner == null)
            {
                await Application.Current.MainPage.DisplayAlert("Bilgi", "İsim ve Sahip seçimi zorunludur.", "Tamam");
                return;
            }

            var pet = new Pet
            {
                Name = Name,
                Species = Species,
                Age = Age,
                Symptom = Symptom,
                OwnerId = SelectedOwner.Id
            };

            await _dbService.SavePetAsync(pet);
            await LoadPetsAsync();
            
            // Navigate back or Clear? Assuming navigation back or logic handled in view.
            // Let's just clear for now.
            Name = string.Empty;
            Species = string.Empty;
            Age = 0;
            Symptom = string.Empty;
            SelectedOwner = null;

            await Application.Current.MainPage.DisplayAlert("Başarılı", "Hasta eklendi.", "Tamam");
        }
        
        private async Task DeletePetAsync(Pet pet)
        {
            if(pet == null) return;
            bool confirm = await Application.Current.MainPage.DisplayAlert("Sil", $"{pet.Name} silinsin mi?", "Evet", "Hayır");
            if(confirm)
            {
                await _dbService.DeletePetAsync(pet);
                Pets.Remove(pet);
            }
        }
    }
}