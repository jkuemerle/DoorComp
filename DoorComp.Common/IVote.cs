using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorComp.Common
{
    public struct VoteInfo
    {
        public string DoorID { get; set; }

        public string EventCode { get; set; }

        public string VoterID { get; set; }

        public string Payload { get; set; }
    }

    public interface IVote
    {
        bool PostVote(VoteInfo Vote);
    }
}
