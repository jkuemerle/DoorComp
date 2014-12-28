using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceHost;

namespace DoorComp.DTO
{
    [Route("/Claim")]
    [Route("/Claim/{DoorID}")]
    [Route("/Claim/{DoorID}/{Name}")]
    [Route("/Claim/{DoorID}/{Name}/{EmailAddress}")]
    public class Claim
    {
        public string DoorID { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }
    }

    public class ClaimResponse
    {
        public bool Status { get; set; }
    }
}
