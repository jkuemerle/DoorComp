using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Caching;

using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using DoorComp.Common;
using ServiceStack.Common.Web;

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
            string key = string.Format("Door:{0}", request.DoorID);
            if (cache.Contains(key))
                return cache.Get(key);
            var pic = ((IPictureSource)HttpContext.Current.Application["PhotoSource"]).GetPicture(request.DoorID);
            var claim = ((IClaimSource)HttpContext.Current.Application["ClaimSource"]).GetClaim(request.DoorID);
            var door = ((IDoorSource)HttpContext.Current.Application["DoorSource"]).GetDoor(request.DoorID);
            if(null == pic )
                throw HttpError.NotFound(string.Format("Cannot find door {0}",request.DoorID));
            var resp = new DoorResponse() { DoorID = request.DoorID, 
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
            return resp;
        }
    }
}