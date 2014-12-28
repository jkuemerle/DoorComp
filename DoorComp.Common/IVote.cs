using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace DoorComp.Common
{
    [Serializable]
    public class VotePayload
    {
        public string IP { get; set; }
        public NameValueCollection Headers { get; set; }

        public IDictionary<string, string> Cookies { get; set; }

        public VotePayload()
        {
            this.Headers = new NameValueCollection();
            this.Cookies = new Dictionary<string, string>();
        }
    }

    [Serializable]
    public class VoteInfo
    {
        public string DoorID { get; set; }

        public string VoterID { get; set; }

        public VotePayload Payload { get; set; }

        public VoteInfo()
        {
            this.Payload = new VotePayload();
        }
    }

    public interface IVote
    {
        bool PostVote(VoteInfo Vote);

        string GetVoteCount(string DoorID);
    }
}
