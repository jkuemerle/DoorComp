using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceHost;
using DoorComp.Common;

namespace DoorComp.DTO
{
    [Route("/DoorAdmin/{DoorID}")]
    public class DoorAdmin
    {
        public string DoorID { get; set; }
    }

    public class DoorAdminResponse
    {
        public string DoorID { get; set; }
        public PictureInfo Picture { get; set; }
        public string VoteURL { get; set; }

        public string ClaimURL { get; set; }

        public DoorInfo DoorDetails { get; set; }

        public ClaimInfo ClaimDetails { get; set; }

        public string Votes { get; set; }
    }

}
