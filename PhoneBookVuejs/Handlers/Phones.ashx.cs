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
    /// Обработчик GET-запросов возвращающий телефоны по указанному contactId.
    /// ?contactId - обязательный параметр в query string.
    /// </summary>
    public class Phones : IHttpHandler
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
                IQueryable<Phone> query = db.Phones;
                string contactIdString = request.QueryString["contactId"];

                if (contactIdString == null)
                {
                    response.StatusCode = 404;
                    response.AddHeader("Error-Message", "There is no contactId in QueryString");
                    return;
                }

                int? contactId = HandlerHelper.ValidIdOrNull(contactIdString);

                if (contactId == null)
                {
                    response.StatusCode = 404;
                    response.AddHeader("Error-Message", "Specified contactId isn't valid");
                    return;
                }

                FilterPhonesByContactId((int)contactId, ref query);
                

                Phone[] phoneArray = query.ToArray<Phone>();
                response.AttachAsJson<Phone[]>(phoneArray);
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

        private void FilterPhonesByContactId(int contactId, ref IQueryable<Phone> query)
        {
            query = query.Where(phone => phone.ContactId == contactId);
        }
    }
}