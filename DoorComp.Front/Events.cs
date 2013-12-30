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

    [Route("/Events")]
    [Route("/Events/{Status}")]
    public class Event
    {
        public string Status { get; set; }
    }

    public class EventsResponse
    {
        public List<EventInfo> Events { get; set; }
    }

    [ClientCanSwapTemplates]
    [DefaultView("Events")]
    public class EventsService : Service
    {
        private static ObjectCache cache = MemoryCache.Default;
        private const int cacheSeconds = 240;
        private static object cacheLock = new object();

        public object Get(Event request)
        {
            try
            {
                EventStatus stat = EventStatus.Active;
                if (null != request.Status)
                    Enum.TryParse(request.Status, false, out stat);
                string key = string.Format("Events:{0}", stat);
                if (cache.Contains(key))
                {
                    Log.Information("Feature", "List Events", "Events with a status of {0} were served from cache.", stat.ToString());
                    return cache.Get(key);
                }
                var ev = ((IEventSource)HttpContext.Current.Application["EventSource"]).ListEvents(stat).ToList();
                if (null == ev)
                    throw HttpError.NotFound(string.Format("There are currently no {0} events listed ", request.Status));
                var ret = new EventsResponse() { Events = ev };
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
                Log.Information("Feature", "List Events", "Events with a status of {0} were served from database.", stat.ToString());
                return ret;
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Error", "List Events", "Error when listing {0} events", request.Status);
                throw;
            }
        }
    }
}