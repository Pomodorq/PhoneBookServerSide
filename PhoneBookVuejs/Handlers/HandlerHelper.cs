using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneBookVuejs.Handlers
{

    public static class HandlerHelper
    {
        /// <summary>
        /// Проверяет idString на валидность. (int, >=1)
        /// Если все ок, возвращает его.
        /// В противном случае возвращает null.
        /// </summary>
        public static int? ValidIdOrNull(string idString)
        {
            bool isInt = Int32.TryParse(idString, out int id);
            if (!isInt || id <= 0)
            {
                return null;
            }
            return id;
        }
    }

    
}