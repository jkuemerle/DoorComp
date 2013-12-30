using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Caching;

using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.Common.Web;
using Gibraltar.Agent;

using DoorComp.Common;


namespace DoorComp.Front
{

    [Route("/Doors/{EventCode}")]
    public class Doors
    {
        public string EventCode { get; set; }

        public override bool Equals(object obj)
        {
            Doors test = (Doors)obj;
            return this.EventCode.Equals(test.EventCode);
        }
    }

    public class DoorsResponse
    {
        public EventInfo Event { get; set; }
        public List<PictureInfo> Pictures { get; set; }

        public Dictionary<string, string> VoteURL { get; set; }

        public Dictionary<string, string> ClaimURL { get; set; }

    }

    [ClientCanSwapTemplates]
    [DefaultView("Doors")]
    public class DoorsService : Service
    {
        private static ObjectCache cache = MemoryCache.Default;
        private const int cacheSeconds = 240;
        private static object cacheLock = new object();
        public object Get(Doors request)
        {
            try
            {
                string key = string.Format("Doors:{0}", request.EventCode);
                if (cache.Contains(key))
                {
                    Log.Information("Feature", "List Doors", "Doors for {0} were served from cache.", request.EventCode);
                    return cache.Get(key);
                }
                var ev = ((IEventSource)HttpContext.Current.Application["EventSource"]).GetEvent(request.EventCode);
                if (null == ev)
                    throw HttpError.NotFound(string.Format("Cannot find event code {0}", request.EventCode));
                var ret = new DoorsResponse() { Event = ev };
                ret.Pictures = ((IPictureSource)HttpContext.Current.Application["PhotoSource"]).ListPictures(string.Format("doorcomp,{0}", request.EventCode)).ToList();
                ret.VoteURL = (from a in ret.Pictures select new { ID = a.ID, URL = string.Format("/Vote/{0}", a.ID) }).ToDictionary(x => x.ID, x => x.URL);
                ret.ClaimURL = (from a in ret.Pictures select new { ID = a.ID, URL = string.Format("/Claim/{0}", a.ID) }).ToDictionary(x => x.ID, x => x.URL);
                if (!cache.Contains(key))
                {
                    lock (cacheLock)
                    {
                        if (!cache.Contains(key))
                        {
                            var policy = new CacheItemPolicy() { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(cacheSeconds) };
                            cache.Add(key, ret, policy);
                        }
                    }
                }
                Log.Information("Feature", "List Doors", "Doors for {0} were served from database.", request.EventCode);
                return ret;
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Error", "List Doors", "Error when listing doors for event {0}", request.EventCode);
                throw;
            }
        }
    }
}