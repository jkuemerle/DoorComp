using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using ServiceStack;
using ServiceStack.ServiceHost;

namespace DoorComp.Front
{
    public class Global : System.Web.HttpApplication
    {
        
        protected void Application_Start(object sender, EventArgs e)
        {
            new AppHost().Init();
            var creds = System.IO.File.ReadAllLines(@"c:\temp\flickrcred.txt");
            FlickrSource.FlickrSource src = null;
            if (creds.Length > 1)
            {
                src = new FlickrSource.FlickrSource(creds[0], creds[1]);
            }
            this.Application.Add("PhotoSource", src);
            this.Application.Add("EventSource", new EventSource.Mock.MockEventSource());
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}