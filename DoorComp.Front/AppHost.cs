using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Funq;
using ServiceStack.DataAnnotations;
using ServiceStack.Logging;
using ServiceStack.Logging.Support.Logging;
using ServiceStack.OrmLite;
using ServiceStack.Razor;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;
namespace DoorComp.Front
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base("Door Competition Service", typeof(AppHost).Assembly) {
        }

        public override void Configure(Container container)
        {
            Plugins.Add(new RazorFormat());
        }
    }
}