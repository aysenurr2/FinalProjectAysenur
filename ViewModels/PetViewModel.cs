using System.Collections.ObjectModel;
using FinalProjectAysenur.Models;
using FinalProjectAysenur.Services;

namespace FinalProjectAysenur.ViewModels
{
    public class PetViewModel
    {
        private readonly DatabaseService _dbService;

        
        public ObservableCollection<Pet> Pets { get; set; } = new();

        public PetViewModel(DatabaseService dbService)
        {
            _dbService = dbService;
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

        
        public async Task AddPetAsync(Pet pet)
        {
            await _dbService.SavePetAsync(pet);

            
            Pets.Add(pet);
        }
    }
}