using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using DoorComp.Common;
using ServiceStack.Common.Web;

namespace DoorComp.Front
{
    [Route("/Claim/{DoorID}")]
    public class Claim
    {
        public string DoorID { get; set; }
    }

    public class ClaimResponse
    {
        public bool Status {get;set;}
    }

    public class ClaimService : Service
    {
        public object Get(Claim request)
        {
            return new ClaimResponse() { Status = true };
        }
    }
}