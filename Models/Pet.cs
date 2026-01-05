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

        public string Species { get; set; }

        public string Breed { get; set; }

        public int Age { get; set; }

        public string OwnerName { get; set; }

        public string OwnerPhone { get; set; }
    }
}
