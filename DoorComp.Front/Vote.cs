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
            if(string.IsNullOrEmpty(base.Request.GetCookieValue("VoterID")))
            {
                base.Response.Cookies.AddPermanentCookie("VoterID", Guid.NewGuid().ToString(), false);
            }
            return new VoteResponse()
            {
                Status = string.Format(
                    "Welcome to the Door Competition voting service. You can post your vote to this endpoint. DoorID: {0}", request.DoorID)
            };
        }

        public object Post(Vote request)
        {
            if (!string.IsNullOrEmpty(request.DoorID) && null != _voteSource)
            {
                var req = base.Request;
                string voterID = req.Cookies.ContainsKey("VoterID") ? req.Cookies["VoterID"].Value : null;
                var payload = new VotePayload() { IP = req.RemoteIp, Cookies = req.CookiesAsDictionary(), Headers = req.Headers };
                if (string.IsNullOrEmpty(voterID))
                {
                    base.Response.Cookies.AddPermanentCookie("VoterID", Guid.NewGuid().ToString(), false);
                }
                _voteSource.PostVote(new VoteInfo() { DoorID = request.DoorID, Payload = payload, VoterID = voterID });
            }

            return new VoteResponse() { Status = "Success" };
        }
    }
}