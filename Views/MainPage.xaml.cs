using FinalProjectAysenur.ViewModels;
using FinalProjectAysenur.Views;

namespace FinalProjectAysenur.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly PetViewModel _viewModel;

        public MainPage(PetViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadPetsAsync();
        }

        private async void OnAddPetClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AddPetPage));
        }
    }
}