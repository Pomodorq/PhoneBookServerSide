using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneBookVuejs.Handlers
{
    public static class ResponseAttachExtension
    {
        public static void AttachAsJson<T>(this HttpResponse response, T obj)
        {
            string output = JsonConvert.SerializeObject(obj);
            response.ContentType = "application/json;charset=utf-8";
            response.Write(output);
        }
    }
}