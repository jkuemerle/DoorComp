using System.Collections.Generic;
using System.Linq;
using System.Web;

using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

using IPictureSource;


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
        public string EventCode { get; set; }
        public List<PictureInfo> Pictures { get; set; }
    }

    [ClientCanSwapTemplates]
    [DefaultView("Doors")]
    public class DoorsService : Service
    {
        public object Get(Doors request)
        {
            var ret = new DoorsResponse() { EventCode = request.EventCode };
            ret.Pictures = ((IPictureSource.IPictureSource)HttpContext.Current.Application["PhotoSource"]).ListPictures(string.Format("doorcomp,{0}", request.EventCode)).ToList();
            return ret;
        }
    }
}