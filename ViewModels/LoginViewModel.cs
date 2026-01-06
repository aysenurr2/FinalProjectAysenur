using FinalProjectAysenur.Services;
using FinalProjectAysenur.Views;
using System.Windows.Input;

namespace FinalProjectAysenur.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly DatabaseService _dbService;

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel(DatabaseService dbService)
        {
            _dbService = dbService;
            LoginCommand = new Command(async () => await LoginAsync());
        }

        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Hata", "Lütfen tüm alanları doldurun.", "Tamam");
                return;
            }

            var clinic = await _dbService.LoginOrRegisterAsync(Email, Password);

            if (clinic != null)
            {
                // Giriş başarılı, AppShell'e yönlendir (veya MainPage'e)
                // App.xaml.cs içinde MainPage AppShell olarak ayarlanmalı.
                // Şimdilik sadece AppShell'e gitmeyi deneyelim.
                Application.Current.MainPage = new AppShell();
            }
        }
    }
}
