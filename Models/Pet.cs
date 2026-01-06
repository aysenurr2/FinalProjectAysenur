using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace FinalProjectAysenur.Models
{
    public class Pet
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; } // Cins/Tür
        public int Age { get; set; }
        public string Symptom { get; set; } // Şikayet
        public int OwnerId { get; set; } // İlişki: Hangi sahibe ait?
        public bool IsActive { get; set; } = true;
    }
}
