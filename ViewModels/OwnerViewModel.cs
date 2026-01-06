using System.Collections.ObjectModel;
using System.Windows.Input;
using FinalProjectAysenur.Models;
using FinalProjectAysenur.Services;

namespace FinalProjectAysenur.ViewModels
{
    public class OwnerViewModel : BaseViewModel
    {
        private readonly DatabaseService _dbService;

        public ObservableCollection<Owner> Owners { get; set; } = new();

        private string _fullName;
        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        private string _phone;
        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public ICommand SaveOwnerCommand { get; }
        public ICommand DeleteOwnerCommand { get; }
        public ICommand LoadOwnersCommand { get; }

        public OwnerViewModel(DatabaseService dbService)
        {
            _dbService = dbService;
            SaveOwnerCommand = new Command(async () => await SaveOwnerAsync());
            DeleteOwnerCommand = new Command<Owner>(async (o) => await DeleteOwnerAsync(o));
            LoadOwnersCommand = new Command(async () => await LoadOwnersAsync());
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

        private async Task SaveOwnerAsync()
        {
            if (string.IsNullOrWhiteSpace(FullName))
            {
                await Application.Current.MainPage.DisplayAlert("Hata", "İsim boş olamaz", "Tamam");
                return;
            }

            var newOwner = new Owner
            {
                FullName = FullName,
                Phone = Phone,
                Email = Email
            };

            await _dbService.SaveOwnerAsync(newOwner);
            await LoadOwnersAsync();

            FullName = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
        }

        private async Task DeleteOwnerAsync(Owner owner)
        {
            if (owner == null) return;
            bool confirm = await Application.Current.MainPage.DisplayAlert("Onay", $"{owner.FullName} silinsin mi?", "Evet", "Hayır");
            if (confirm)
            {
                await _dbService.DeleteOwnerAsync(owner);
                await LoadOwnersAsync();
            }
        }
    }
}
