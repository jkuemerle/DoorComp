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

    [Route("/Doors/{EventCode}")]
    public class Doors
    {
        public string EventCode { get; set; }
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
        public object Get(Doors request)
        {
            var ev = ((IEventSource)HttpContext.Current.Application["EventSource"]).GetEvent(request.EventCode);
            if(null == ev)
                throw HttpError.NotFound(string.Format("Cannot find event code {0}",request.EventCode));
            var ret = new DoorsResponse() { Event = ev };
            ret.Pictures = ((IPictureSource)HttpContext.Current.Application["PhotoSource"]).ListPictures(string.Format("doorcomp,{0}", request.EventCode)).ToList();
            ret.VoteURL = (from a in ret.Pictures select new { ID = a.ID, URL = string.Format("/Vote/{0}", a.ID) }).ToDictionary(x => x.ID, x => x.URL);
            ret.ClaimURL = (from a in ret.Pictures select new { ID = a.ID, URL = string.Format("/Claim/{0}", a.ID) }).ToDictionary(x => x.ID, x => x.URL);
            return ret;
        }
    }
}