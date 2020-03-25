using PhoneBookVuejs.Models;
using PhoneBookVuejs.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;

namespace PhoneBookVuejs.Handlers
{
    /// <summary>
    /// Обработчик запросов добавления контакта в БД.
    /// Ожидает POST-запрос с объектом FormData(form/multipart)
    /// с двумя полями: Name, Surname.
    /// </summary>
    public class ContactPost : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;
            var response = context.Response;
            response.AppendHeader("Access-Control-Allow-Origin", "*");

            if (request.RequestType != "POST")
            {
                response.StatusCode = 405;
                response.AppendHeader("Error-Message", "Only POST method is allowed");
                return;
            }

            using (ContactContext db = new ContactContext())
            {
                if (String.IsNullOrEmpty(request.Form.Get("Name")))
                {
                    response.StatusCode = 404;
                    response.AppendHeader("Error-Message", "Name - required field");
                    return;
                }

                Contact newContact = MakeContact(request.Form);
                AddContact(db, newContact);
                response.AttachAsJson<Contact>(newContact);
                response.StatusCode = 200;
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        
        // Создает контакт из формы.
        private Contact MakeContact(NameValueCollection contactForm)
        {
            Contact newContact = new Contact { Name = contactForm.Get("Name"), Surname = contactForm.Get("Surname") };
            return newContact;
        }

        // Добавляет контакт в БД.
        private void AddContact(ContactContext db, Contact newContact)
        {
            db.Contacts.Add(newContact);
            db.SaveChanges();
        }
    }
}