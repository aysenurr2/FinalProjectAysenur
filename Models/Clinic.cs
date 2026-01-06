using SQLite;

namespace FinalProjectAysenur.Models
{
    public class Clinic
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Email { get; set; } // Giriş için kullanılacak mail
        public string Password { get; set; } // Şifre
        public string ClinicName { get; set; } // Klinik Adı
        public string DoctorName { get; set; } // Başhekim/Doktor Adı
    }
}