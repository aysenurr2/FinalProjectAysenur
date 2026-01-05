namespace FinalProjectAysenur.Views;

public partial class AddPetPage : ContentPage
{
    private readonly Services.DatabaseService _dbService;

    public AddPetPage(Services.DatabaseService dbService)
    {
        InitializeComponent();
        _dbService = dbService;
    }

    // Hatanýn çözümü burasýdýr: Ýsim ve parametreler (sender, e) birebir tutmalý
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(PetNameEntry.Text)) return;

        var newPet = new Models.Pet
        {
            Name = PetNameEntry.Text,
            Species = SpeciesEntry.Text,
           
        };

        await _dbService.SavePetAsync(newPet);
        await DisplayAlert("Baþarýlý", "Hasta kaydý eklendi!", "Tamam");
        await Navigation.PopAsync(); // Ana sayfaya geri döner
    }
}