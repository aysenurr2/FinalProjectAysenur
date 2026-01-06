using SQLite;

namespace FinalProjectAysenur.Models
{
    public class Owner
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; } = true;
    }
}