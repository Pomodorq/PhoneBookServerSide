using PhoneBookVuejs.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace PhoneBookVuejs.Handlers
{
    /// <summary>
    /// Обработчик POST-запросов добавления телефона в БД.
    /// Ожидает запрос с FormData(form/multipart),
    /// с двумя полями PhoneNumber и ContactId
    /// Возвращает добавленный телефон в формате json.
    /// </summary>
    public class PhonePost : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;
            var response = context.Response;
            response.AppendHeader("Access-Control-Allow-Origin", "*");

            if (request.RequestType != "POST")
            {
                response.StatusCode = 405;
                response.AddHeader("Error-Message", "Only POST method is allowed");
                return;
            }
            
            using (ContactContext db = new ContactContext())
            {
                var form = request.Form;
                string contactIdStr = form.Get("ContactId");
                int? contactId = HandlerHelper.ValidIdOrNull(contactIdStr);
                
                if (contactId == null)
                {
                    response.StatusCode = 404;
                    response.AppendHeader("Error-Message", "Specified contactId isn't valid");
                    return;
                }

                Phone newPhone = new Phone 
                { 
                    PhoneNumber = form.Get("PhoneNumber"),
                    ContactId = (int)contactId 
                };

                db.Phones.Add(newPhone);
                db.SaveChanges();
                response.AttachAsJson<Phone>(newPhone);
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
    }
}