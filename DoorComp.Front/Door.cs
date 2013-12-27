using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }

    public class DoorResponse
    {
        public string DoorID { get; set; }
        public EventInfo Event{ get; set; }
        public PictureInfo Picture { get; set; }
        public string VoteURL { get; set; }

        public string ClaimURL { get; set; }

        public DoorInfo Details { get; set; }
    }

    [ClientCanSwapTemplates]
    [DefaultView("Door")]
    public class DoorService : Service
    {
        public object Get(Door request)
        {
            var ev = ((IEventSource)HttpContext.Current.Application["EventSource"]).GetEvent(request.EventCode);
            var pic = ((IPictureSource)HttpContext.Current.Application["PhotoSource"]).GetPicture(request.DoorID);
            var claim = ((IClaimSource)HttpContext.Current.Application["ClaimSource"]).GetClaim(request.DoorID);
            var door = ((IDoorSource)HttpContext.Current.Application["DoorSource"]).GetDoor(request.DoorID);
            if(null == pic )
                throw HttpError.NotFound(string.Format("Cannot find door {0}",request.DoorID));
            return new DoorResponse() { DoorID = request.DoorID, Event = ev, 
                Picture = pic, 
                VoteURL = string.Format("/Vote/{0}", request.DoorID),
                ClaimURL = string.Format("/Claim/{0}", request.DoorID),
                Details = door
            };
        }
    }
}