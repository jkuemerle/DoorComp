using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ServiceStack.ServiceHost;
using DoorComp.Common;
namespace DoorComp.DTO
{
    [Route("/Door/{EventCode}/{DoorID}")]
    [Route("/Door/{DoorID}")]
    public class Door
    {
        public string EventCode { get; set; }
        public string DoorID { get; set; }

        public override bool Equals(object obj)
        {
            var test = (Door)obj;
            return this.EventCode.Equals(test.EventCode) && this.DoorID.Equals(test.DoorID);
        }
    }

    public class DoorResponse
    {
        public string DoorID { get; set; }
        public PictureInfo Picture { get; set; }
        public string VoteURL { get; set; }

        public string ClaimURL { get; set; }

        public DoorInfo DoorDetails { get; set; }

        public ClaimInfo ClaimDetails { get; set; }

    }

}
