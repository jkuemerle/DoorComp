using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.Common.Web;
using Gibraltar.Agent;
using DoorComp.Common;
using DoorComp.DTO;

namespace DoorComp.Front
{

    [ClientCanSwapTemplates]
    [DefaultView("Vote")]
    public class VoteService : Service
    {
        public static IVote _voteSource;
        public static object _voteSourceLock = new object();

        static VoteService()
        {
            if (null == _voteSource)
            {
                lock (_voteSourceLock)
                {
                    if (null == _voteSource)
                        _voteSource = (IVote)HttpContext.Current.Application["VoteSource"];
                }
            }
        }
        public object Get(Vote request)
        {
            try
            {
                string voterID = base.Request.CookiesAsDictionary().ContainsKey("VoterID") ? base.Request.CookiesAsDictionary()["VoterID"] : null;
                Log.Information("Feature", "Get Vote", "A get for door {0} from IP: {1}, ID: {2}", request.DoorID, base.Request.RemoteIp, voterID);
                if (string.IsNullOrEmpty(voterID))
                {
                    base.Response.Cookies.AddPermanentCookie("VoterID", Guid.NewGuid().ToString(), false);
                }
                return new VoteResponse()
                {
                    Status = string.Format(
                        "Welcome to the Door Competition voting service. You can post your vote to this endpoint. DoorID: {0}", request.DoorID)
                };
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Error", "Get Vote", "Error when getting vote for door {0}", request.DoorID);
                throw;
            }
        }

        public object Post(Vote request)
        {
            try
            {
                if (!string.IsNullOrEmpty(request.DoorID) && null != _voteSource)
                {
                    var req = base.Request;
                    string voterID = req.Cookies.ContainsKey("VoterID") ? req.Cookies["VoterID"].Value : null;
                    var payload = new VotePayload() { IP = req.RemoteIp, Cookies = req.CookiesAsDictionary(), Headers = req.Headers };
                    Log.Information("Feature", "Post Vote", "A vote for door {0} from IP: {1}, ID: {2}", request.DoorID, payload.IP, voterID);
                    if (string.IsNullOrEmpty(voterID))
                    {
                        base.Response.Cookies.AddPermanentCookie("VoterID", Guid.NewGuid().ToString(), false);
                    }
                    _voteSource.PostVote(new VoteInfo() { DoorID = request.DoorID, Payload = payload, VoterID = voterID });
                }
                else
                {
                    Log.Warning("Warning", "Post Vote", "Posting vote for empty DoorID or no vote source. DoorID: {0}", request.DoorID);
                }
                return new VoteResponse() { Status = "Success" };
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Error", "Post Vote", "Error posting vote for door {0}", request.DoorID);
                throw;
            }
        }
    }
}