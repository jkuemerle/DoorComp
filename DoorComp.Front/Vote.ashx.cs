using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DoorComp.Common;

namespace DoorComp.Front
{
    /// <summary>
    /// Summary description for Vote
    /// </summary>
    public class Vote : IHttpHandler
    {
        public static IVote _voteSource;
        public static object _voteSourceLock = new object();

        public void ProcessRequest(HttpContext context)
        {
            if(null == _voteSource)
            {
                lock(_voteSourceLock)
                {
                    if (null == _voteSource)
                        _voteSource = (IVote)HttpContext.Current.Application["VoteSource"];
                }
            }
            switch(context.Request.HttpMethod)
            {
                case "POST":
                    var vi = ParsePayload(context.Request);
                    context.Response.StatusCode = 202;
                    context.Response.ContentType = "application/json";
                    context.Response.Write("{status: \"Accepted\"");
                    if(!string.IsNullOrEmpty(vi.DoorID))
                    {
                        _voteSource.PostVote(vi);
                    }
                    break;
                default:
                    var v = ParsePayload(context.Request);
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(string.Format("Welcome to the Door Competition voting service. You can post your vote to this endpoint. DoorID: {0}",v.DoorID));
                    break;
            }
        }

        private VoteInfo ParsePayload(HttpRequest Request)
        {
            var vi = new VoteInfo();
            vi.DoorID = Request.Params["DoorID"];
            vi.EventCode = Request.Params["EventCode"];
            vi.Payload.Headers = Request.Headers;
            vi.Payload.IP = Request.UserHostAddress;
            return vi;
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}