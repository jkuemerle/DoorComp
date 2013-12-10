using System.Collections.Generic;
using System.Linq;
using System.Web;

using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using IPictureSource;
using ServiceStack.Common.Web;


namespace DoorComp.Front
{

    [Route("/Doors/{EventCode}")]
    public class Doors
    {
        public string EventCode { get; set; }
        public string DoorID { get; set; }
    }

    public class DoorsResponse
    {
        public EventInfo Event { get; set; }
        public List<PictureInfo> Pictures { get; set; }
    }

    [ClientCanSwapTemplates]
    [DefaultView("Doors")]
    public class DoorsService : Service
    {
        public object Get(Doors request)
        {
            var ev = ((IPictureSource.IEventSource)HttpContext.Current.Application["EventSource"]).GetEvent(request.EventCode);
            if(null == ev)
                throw HttpError.NotFound(string.Format("Cannot find event code {0}",request.EventCode));
            var ret = new DoorsResponse() { Event = ev };
            ret.Pictures = ((IPictureSource.IPictureSource)HttpContext.Current.Application["PhotoSource"]).ListPictures(string.Format("doorcomp,{0}", request.EventCode)).ToList();
            return ret;
        }
    }
}