﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using DoorComp.Common;
using ServiceStack.Common.Web;
using Gibraltar.Agent;
using DoorComp.DTO;

namespace DoorComp.Front
{

    public class ClaimService : Service
    {
        public object Get(Claim request)
        {
            try
            {
                Log.Information("Feature", "Get Claim", "Claim: {0}.", request.ToString());
                return new ClaimResponse() { Status = true };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error", "Get Claim", "Error when getting claim: {0} ", request.ToString());
                throw;
            }
        }

        public object Post(Claim request)
        {
            try
            {
                Log.Information("Feature", "Make Claim", "Door {0} was claimed by {1} at {2}.", request.DoorID, request.Name, request.EmailAddress);
                IClaimSource cs = (IClaimSource)HttpContext.Current.Application["ClaimSource"];
                if (null == cs)
                    throw new Exception(string.Format("No ClaimSource, unable to make claim."));
                var stat = cs.Claim(request.DoorID, new ClaimInfo() { DoorID = request.DoorID, Name = request.Name, Email = request.EmailAddress });
                if(stat)
                {
                    Common.ExpireDoor(request.DoorID);
                }
                return new ClaimResponse() { Status = stat };
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Error", "Make Claim", "Error when {0} at {1} tried to claim door {2}", request.Name, request.EmailAddress, request.DoorID);
                throw;
            }
        }


    }
}