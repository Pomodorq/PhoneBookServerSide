using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PhoneBookVuejs.Models
{
    public class ContactDatabaseInitializer : DropCreateDatabaseIfModelChanges<ContactContext>
    {
        protected override void Seed(ContactContext context)
        {
            GetContacts().ForEach(c => context.Contacts.Add(c));
            GetPhones().ForEach(p => context.Phones.Add(p));
        }

        public static List<Contact> GetContacts()
        {
            var contacts = new List<Contact>
            {
                new Contact
                {
                    Name = "Жора",
                    Surname = "Ромов",
                },
                new Contact
                {
                    Name = "Рома",
                    Surname = "Жоров"
                },
                new Contact
                {
                    Name = "Вася",
                    Surname = "Васильев"
                }
            };
            return contacts;
        }

        public static List<Phone> GetPhones()
        {
            var phones = new List<Phone>
            {
                new Phone
                {
                    PhoneNumber = "89003003020",
                    ContactId = 1
                },
                new Phone
                {
                    PhoneNumber = "89005006677",
                    ContactId = 1
                },
                new Phone
                {
                    PhoneNumber = "15155151555",
                    ContactId = 2
                },
                new Phone
                {
                    PhoneNumber = "44214214214",
                    ContactId = 2
                },
                new Phone
                {
                    PhoneNumber = "65436543665",
                    ContactId = 2
                },
                new Phone
                {
                    PhoneNumber = "75436543654",
                    ContactId = 3
                },
                new Phone
                {
                    PhoneNumber = "86576365436",
                    ContactId = 3
                },
            };
            return phones;
        }
    }
}