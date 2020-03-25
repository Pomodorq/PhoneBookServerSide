using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using PhoneBookVuejs.Models;

namespace PhoneBookVuejs.Handlers
{
    /// <summary>
    /// Обработчик POST-запросов добавления нескольких телефонов.
    /// Ожидает тело запроса содержащее массив телефонов в json формате.
    /// </summary>
    public class PhonesPost : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;
            var response = context.Response;
            response.AppendHeader("Access-Control-Allow-Origin", "*");


            Phone[] phones;
            JsonSerializer serializer = new JsonSerializer();
            using (var sr = new StreamReader(request.InputStream))
            using (var jr = new JsonTextReader(sr))
            {
                phones = serializer.Deserialize<Phone[]>(jr);
            }

            using (ContactContext db = new ContactContext())
            {
                db.Phones.AddRange(phones);
                db.SaveChanges();
                response.AttachAsJson<Phone[]>(phones);
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