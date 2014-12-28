using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Caching;

using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.Common.Web;
using Gibraltar.Agent;

using DoorComp.Common;
using DoorComp.DTO;

namespace DoorComp.Front
{
    [ClientCanSwapTemplates]
    [DefaultView("DoorAdmin")]
    [Authenticate]
    public class DoorAdminService : Service
    {
        public object Get(DoorAdmin request)
        {
            try
            {
                var pic = ((IPictureSource)HttpContext.Current.Application["PhotoSource"]).GetPicture(request.DoorID);
                var claim = ((IClaimSource)HttpContext.Current.Application["ClaimSource"]).GetClaim(request.DoorID);
                var door = ((IDoorSource)HttpContext.Current.Application["DoorSource"]).GetDoor(request.DoorID);
                var votes = ((IVote)HttpContext.Current.Application["VoteSource"]).GetVoteCount(request.DoorID);
                if (null != door)
                {
                    string evkey = string.Format("EventID:{0}", door.EventID);
                    var ev = ((IEventSource)HttpContext.Current.Application["EventSource"]).GetEventByID(door.EventID.ToString());
                    door.Event = ev;
                }
                if (null == pic)
                {
                    Log.Error("Error", "Get Door", "Door {0} was requested and not found.", request.DoorID);
                    throw HttpError.NotFound(string.Format("Cannot find door {0}", request.DoorID));
                }
                var resp = new DoorAdminResponse()
                {
                    DoorID = request.DoorID,
                    Picture = pic,
                    VoteURL = string.Format("/Vote/{0}", request.DoorID),
                    ClaimURL = string.Format("/Claim/{0}", request.DoorID),
                    DoorDetails = door,
                    ClaimDetails = claim,
                    Votes = votes
                };
                return resp;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error", "Get Door", "Error when requesting admin for door {0}", request.DoorID);
                throw;
            }

        }
    }
}