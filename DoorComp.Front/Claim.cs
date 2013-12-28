﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using DoorComp.Common;
using ServiceStack.Common.Web;
using Gibraltar.Agent;

namespace DoorComp.Front
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
        public bool Status {get;set;}
    }

    public class ClaimService : Service
    {
        public object Get(Claim request)
        {
            Log.Information("Feature", "Get Claim", "Claim: {0}.", request);
            return new ClaimResponse() { Status = true };
        }

        public object Post(Claim request)
        {
            Log.Information("Feature", "Make Claim", "Door {0} was claimed by {1} at {2}.", request.DoorID, request.Name, request.EmailAddress);
            IClaimSource cs = (IClaimSource)HttpContext.Current.Application["ClaimSource"];
            if(null == cs)
                throw HttpError.NotFound(string.Format("Unable to make claim."));
            var stat = cs.Claim(request.DoorID, new ClaimInfo() {DoorID = request.DoorID, Name = request.Name, Email = request.EmailAddress});
            return new ClaimResponse() { Status = stat};
        }
    }
}