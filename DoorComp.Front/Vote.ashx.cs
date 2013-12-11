using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoorComp.Front
{
    /// <summary>
    /// Summary description for Vote
    /// </summary>
    public class Vote : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            switch(context.Request.HttpMethod)
            {
                case "POST":
                    context.Response.StatusCode = 202;
                    context.Response.ContentType = "application/json";
                    break;
                default:
                    break;
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}