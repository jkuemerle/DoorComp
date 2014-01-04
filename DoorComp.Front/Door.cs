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
    [Route("/Door/{EventCode}/{DoorID}")]
    [Route("/Door/{DoorID}")]
    public class Door
    {
        public string EventCode { get; set; }
        public string DoorID { get; set; }

        public override bool Equals(object obj)
        {
            var test = (Door) obj;
            return this.EventCode.Equals(test.EventCode) && this.DoorID.Equals(test.DoorID);
        }
    }

    public class DoorResponse
    {
        public string DoorID { get; set; }
        public PictureInfo Picture { get; set; }
        public string VoteURL { get; set; }

        public string ClaimURL { get; set; }

        public DoorInfo DoorDetails { get; set; }

        public ClaimInfo ClaimDetails { get; set; }

    }

    [ClientCanSwapTemplates]
    [DefaultView("Door")]
    public class DoorService : Service
    {

        private static ObjectCache cache = MemoryCache.Default;
        private const int cacheSeconds = 240;
        private static object cacheLock = new object();
        
        public object Get(Door request)
        {
            try
            {
                string key = string.Format("Door:{0}", request.DoorID);
                if (cache.Contains(key))
                {
                    Log.Information("Feature", "Get Door", "Door {0} was served from cache.", request.DoorID);
                    return cache.Get(key);
                }
                var pic = ((IPictureSource)HttpContext.Current.Application["PhotoSource"]).GetPicture(request.DoorID);
                var claim = ((IClaimSource)HttpContext.Current.Application["ClaimSource"]).GetClaim(request.DoorID);
                var door = ((IDoorSource)HttpContext.Current.Application["DoorSource"]).GetDoor(request.DoorID);
                if(null != door)
                {
                    string evkey = string.Format("EventID:{0}", door.EventID);
                    if (cache.Contains(evkey))
                        door.Event = (EventInfo) cache.Get(evkey);
                    else
                    {
                        var ev = ((IEventSource)HttpContext.Current.Application["EventSource"]).GetEventByID(door.EventID.ToString());
                        door.Event = ev;
                        lock(cacheLock)
                        {
                            if(!cache.Contains(evkey))
                            {
                                var evpolicy = new CacheItemPolicy() { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(480) };
                                cache.Add(evkey, ev, evpolicy);
                            }
                        }
                    }
                }
                if (null == pic)
                {
                    Log.Error("Error", "Get Door", "Door {0} was requested and not found.", request.DoorID);
                    throw HttpError.NotFound(string.Format("Cannot find door {0}", request.DoorID));
                }
                var resp = new DoorResponse()
                {
                    DoorID = request.DoorID,
                    Picture = pic,
                    VoteURL = string.Format("/Vote/{0}", request.DoorID),
                    ClaimURL = string.Format("/Claim/{0}", request.DoorID),
                    DoorDetails = door,
                    ClaimDetails = claim
                };
                if (!cache.Contains(key))
                {
                    lock (cacheLock)
                    {
                        if (!cache.Contains(key))
                        {
                            var policy = new CacheItemPolicy() { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(cacheSeconds) };
                            cache.Add(key, resp, policy);
                        }
                    }
                }
                Log.Information("Feature", "Get Door", "Door {0} was served from database", request.DoorID);
                return resp;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error", "Get Door", "Error when requesting door {0}", request.DoorID);
                throw;
            }

            }
    }
}