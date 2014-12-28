using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceHost;

namespace DoorComp.DTO
{
    [Route("/Vote")]
    [Route("/Vote/{DoorID}")]
    public class Vote
    {
        public string DoorID { get; set; }
    }

    public class VoteResponse
    {
        public string Status { get; set; }
    }
}
