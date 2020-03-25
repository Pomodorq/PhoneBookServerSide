using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhoneBookVuejs.Models
{
    public class Phone
    {
        public int Id { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public int ContactId { get; set; }

        [JsonIgnore]
        public virtual Contact Contact { get; set; }

    }
}