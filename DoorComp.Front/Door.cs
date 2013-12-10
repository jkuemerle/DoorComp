using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using IPictureSource;

namespace DoorComp.Front
{
    [Route("/Door/{EventCode}/{ID}")]
    public class Door
    {
        public string EventCode { get; set; }
        public string DoorID { get; set; }
    }

    public class DoorResponse
    {
        public string EventCode { get; set; }
        public PictureInfo Info { get; set; }
    }

    [ClientCanSwapTemplates]
    [DefaultView("Door")]
    public class DoorService : Service
    {
        public object Get(Door request)
        {
            var ret = new DoorResponse() { EventCode = request.EventCode };
            ret.Info = ((IPictureSource.IPictureSource)HttpContext.Current.Application["PhotoSource"]).GetPicture(request.DoorID);
            //ret.Pictures = .ListPictures(string.Format("doorcomp,{0}", request.EventCode)).ToList();
            return ret;
        }
    }
}