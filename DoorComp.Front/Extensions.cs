using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace System.Web
{
    public static class SystemWebExtensions
    {
        public static string Serialize(this HttpRequest Request)
        {
            return ServiceStack.Text.JsonSerializer.SerializeToString(Request.Headers);            
        }
    }
}