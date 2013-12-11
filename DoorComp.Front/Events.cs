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
        public object Get(Event request)
        {
            EventStatus stat = EventStatus.Active;
            if (null != request.Status)
                Enum.TryParse(request.Status, false, out stat);
            var ev = ((IEventSource)HttpContext.Current.Application["EventSource"]).ListEvents(stat).ToList();
            if (null == ev || ev.Count < 1)
                throw HttpError.NotFound(string.Format("There are currently no {0} events listed ", request.Status));
            var ret = new EventsResponse() { Events = ev};
            return ret;
        }
    }
}