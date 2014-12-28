using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceHost;
using DoorComp.Common;
namespace DoorComp.DTO
{
    [Route("/Doors/{EventCode}")]
    public class Doors
    {
        public string EventCode { get; set; }

        public override bool Equals(object obj)
        {
            Doors test = (Doors)obj;
            return this.EventCode.Equals(test.EventCode);
        }
    }

    public class DoorsResponse
    {
        public EventInfo Event { get; set; }
        public List<PictureInfo> Pictures { get; set; }

        public Dictionary<string, string> VoteURL { get; set; }

        public Dictionary<string, string> ClaimURL { get; set; }

    }

}
