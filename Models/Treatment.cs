using SQLite;

namespace FinalProjectAysenur.Models
{
    public class Treatment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int PetId { get; set; }
        public DateTime Date { get; set; }
        public string ServiceList { get; set; } 
        public decimal TotalAmount { get; set; }
        public string Description { get; set; } 

        // UI Helpers
        [Ignore]
        public string PetName { get; set; }
    }
}
