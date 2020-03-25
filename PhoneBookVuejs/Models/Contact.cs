using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhoneBookVuejs.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Surname { get; set; }
        public string ImagePath { get; set; }

        [JsonIgnore]
        public virtual ICollection<Phone> Phones { get; set; }
    }
}