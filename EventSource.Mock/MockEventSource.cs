using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

using DoorComp.Common;

namespace EventSource.Mock
{
    [Export(typeof(IEventSource))]
    public class MockEventSource : IEventSource
    {
        EventInfo cm = new EventInfo() { Code = "codemash", Description = "Codemash 2013", Status = EventStatus.Active, LogoURL = @"http://codemash.org/Sitefinity/WebsiteTemplates/CodeMash/App_Themes/CodeMash/images/logo-codemash.png"};

        public IList<EventInfo> ListEvents(EventStatus Status)
        {
            var retVal = new List<EventInfo>();
            if(Status == EventStatus.Active)
                retVal.Add(cm);
            return retVal;
        }

        public EventInfo GetEvent(string EventCode)
        {
            if("codemash" == EventCode.Trim().ToLowerInvariant() )
                return cm;
            else
                return null;
        }
    }
}
