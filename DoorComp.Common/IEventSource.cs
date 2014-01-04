using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorComp.Common
{

    [Flags]
    public enum EventStatus
    {
        None = 0,
        Active = 1,
        Closed = 2,
        Archived = 4
    }

    public class EventInfo
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public EventStatus Status { get; set; }

        public string StatusString { get; set; }

        public bool IsOpen { get; set; }
        public string LogoURL { get; set; }
    }
    public interface IEventSource
    {
        IList<EventInfo> ListEvents(EventStatus Status);

        EventInfo GetEvent(string EventCode);

        EventInfo GetEventByID(string ID);
    }

}