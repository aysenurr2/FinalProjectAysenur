using SQLite;

namespace FinalProjectAysenur.Models
{
    public class Finance
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TreatmentId { get; set; } // Link to Treatment
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        
        [Ignore]
        public string PetName { get; set; }
        [Ignore]
        public string OwnerName { get; set; }
        [Ignore]
        public string Description { get; set; } // Usually "Treatment #ID" or ServiceList summary
    }
}
