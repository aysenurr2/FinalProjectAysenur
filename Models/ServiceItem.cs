namespace FinalProjectAysenur.Models
{
    public class ServiceItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; } // "Muayene", "Aşı" etc.
        public bool IsSelected { get; set; } // For UI binding
        
        public string DisplayText => $"{Name} - {Price:C}";
    }
}
