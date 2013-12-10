using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPictureSource
{
    public class PictureInfo
    {
        public string ID { get; set; }
        public string FullSizeURL { get; set; }

        public string ThumbnailURL { get; set; }

        public string URL { get; set; }

        public string Title { get; set; }

    }

    [Flags]
    public enum EventStatus
    {
        Active = 1,
        Closed = 2,
        Archived = 4
    }

    public class EventInfo
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public EventStatus Status{ get; set; }

        public string LogoURL { get; set; }
    }

    public interface IPictureSource
    {
        IList<PictureInfo> ListPictures(string SearchString);

        PictureInfo GetPicture(string ID);       
    }

    public interface IEventSource
    {
        IList<EventInfo> ListEvents(EventStatus Status);

        EventInfo GetEvent(string EventCode);
    }
}
