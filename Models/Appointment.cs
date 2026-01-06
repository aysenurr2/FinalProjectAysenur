using SQLite;
using System;

namespace FinalProjectAysenur.Models
{
    public class Appointment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int PetId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Reason { get; set; }
        public bool IsCancelled { get; set; } = false;

        // UI Helpers
        [Ignore]
        public string PetName { get; set; }
        [Ignore]
        public string TimeDisplay => AppointmentDate.ToString("HH:mm");
        [Ignore]
        public Color StatusColor => IsCancelled ? Colors.Red : (AppointmentDate < DateTime.Now ? Colors.Gray : Colors.Black);
    }
}