using Newtonsoft.Json;
using PhoneBookVuejs.Handlers;
using PhoneBookVuejs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneBookVuejs
{
    /// <summary>
    /// Обработчик GET-запросов всех контактов.
    /// Возвращает массив json.
    /// По умолчанию возвращает все контакты.
    /// С помощью query string можно задавать фильтры.
    /// ?id для возврата конкретного контакта.
    /// ?name для возврата контактов с указанным именем.
    /// ?surname для возврата контактов с указанной фамилией.
    /// Фильтры можно совмещать.
    /// </summary>
    public class Contacts : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;
            var response = context.Response;
            response.AppendHeader("Access-Control-Allow-Origin", "*");

            if (request.RequestType != "GET")
            {
                response.StatusCode = 405;
                response.AddHeader("Error-Message", "Only GET method is allowed");
                return;
            }

            using (ContactContext db = new ContactContext())
            {
                IQueryable<Contact> query = db.Contacts;

                string idString = request.QueryString["id"];
                int? id = HandlerHelper.ValidIdOrNull(idString);
                if (idString != null && id == null)
                {
                    response.StatusCode = 404;
                    response.AppendHeader("Error-Message", "Specified id isn't valid");
                    return;
                }
                if (idString != null && id != null)
                {
                    FilterContactsById((int)id, ref query);
                }
                
                if (request.QueryString["name"] != null)
                {
                    FilterContactsByName(request, ref query);
                }

                if (request.QueryString["surname"] != null)
                {
                    FilterContactsBySurname(request, ref query);
                }

                Contact[] contactArray = query.ToArray<Contact>();
                response.AttachAsJson<Contact[]>(contactArray);
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

        /// <summary>
        ///  Фильтрация по несуществующему в БД id считается успешной, 
        ///  на запрос вернется пустой массив.
        /// </summary>
        public void FilterContactsById(int id, ref IQueryable<Contact> query)
        {
            query = query.Where(contact => contact.Id == id);
        }

        
        public void FilterContactsByName(HttpRequest request, ref IQueryable<Contact> query)
        {
            string name = request.QueryString["name"];
            query = query.Where(contact => contact.Name == name);
        }

        public void FilterContactsBySurname(HttpRequest request, ref IQueryable<Contact> query)
        {
            string surname = request.QueryString["surname"];
            query = query.Where(contact => contact.Surname == surname);
        }
    }
}