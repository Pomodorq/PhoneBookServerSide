using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PhoneBookVuejs.Models
{
    public class ContactContext: DbContext
    {
        public ContactContext()
            : base("PhoneBookVuejs")
        {
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Phone> Phones { get; set; }

    }
}