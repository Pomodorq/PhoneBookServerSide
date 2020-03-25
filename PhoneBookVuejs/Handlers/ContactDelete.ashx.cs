using PhoneBookVuejs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneBookVuejs.Handlers
{
    /// <summary>
    /// Обработчик запросов удаления контакта из БД.
    /// Ожидает POST-запрос с объектом FormData(form/multipart)
    /// с единственным параметром - id удаляемого контакта.
    /// </summary>
    public class ContactDelete : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;
            var response = context.Response;
            response.AppendHeader("Access-Control-Allow-Origin", "*");

            var form = request.Form;
            int? id = HandlerHelper.ValidIdOrNull(form.Get("id"));
            if (id == null)
            {
                response.StatusCode = 404;
                response.AppendHeader("Error-Message", "Specified id isn't valid");
                return;
            }
            using (var db = new ContactContext())
            {
                if (!DeleteContact(db, (int)id))
                {
                    response.StatusCode = 404;
                    response.AppendHeader("Error-Message", "There is no contact with specified id");
                    return;
                }
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Удаляет из БД контакт с указанным id,
        /// возвращает true в случае успеха, false если контакта с таким id не существует.
        /// </summary>
        private bool DeleteContact(ContactContext db, int id)
        {
            Contact deletingContact = FindContact(db, id);
            if (deletingContact == null)
            {
                return false;
            }
            DeletePhones(db, id);
            db.Contacts.Remove(deletingContact);
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Возвращает <see cref="PhoneBookVuejs.Models.Contact"/> с указанным id или
        /// null, если контакт отсутствует.
        /// </summary>
        private Contact FindContact(ContactContext db, int id)
        {
            return db.Contacts.FirstOrDefault<Contact>(c => c.Id == id);
        }

        /// <summary>
        /// Удаляет телефоны контакта, если они есть в БД.
        /// </summary>
        private void DeletePhones(ContactContext db, int id)
        {
            IQueryable<Phone> deletingPhones = FindPhones(db, id);
            db.Phones.RemoveRange(deletingPhones);
            db.SaveChanges();
            
        }
        private IQueryable<Phone> FindPhones(ContactContext db, int id)
        {
            return db.Phones.Where(p => p.ContactId == id);
        }
    }
}