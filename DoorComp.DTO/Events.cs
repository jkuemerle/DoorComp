using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceHost;
using DoorComp.Common;

namespace DoorComp.DTO
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

}
