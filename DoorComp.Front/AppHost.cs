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
using ServiceStack.ServiceInterface.Cors;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;
using System.Net;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
namespace DoorComp.Front
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base("Door Competition Service", typeof(AppHost).Assembly) {
        }

        public override void Configure(Container container)
        {
            Routes.AddFromAssembly(typeof(DoorComp.DTO.Door).Assembly);
            Plugins.Add(new RazorFormat());
            Plugins.Add(new CorsFeature(allowedOrigins: "*",
                allowedMethods: "GET, POST, PUT, DELETE, OPTIONS",
                allowedHeaders: "Content-Type, Authorization"));
            ConfigureAuth(container);
            SetConfig(new EndpointHostConfig
            {
                CustomHttpHandlers = {
                    { HttpStatusCode.NotFound, new RazorHandler("/notfound") }
                }
            });
        }

        private void ConfigureAuth(Container container)
        {
            Plugins.Add(new AuthFeature(() => new AuthUserSession(), new IAuthProvider[] { 
                new CredentialsAuthProvider()
            }));
            Plugins.Add(new RegistrationFeature());
            container.Register<ICacheClient>(new MemoryCacheClient());
            var userRep = new InMemoryAuthRepository();
            userRep.CreateUserAuth(new UserAuth() { UserName = "foo", Id = 1 }, "bar");
            container.Register<IUserAuthRepository>(userRep);
        }
    }
}