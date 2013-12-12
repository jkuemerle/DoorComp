using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DoorComp.Common;

namespace VoteSource.Mock
{
    public class MockVoteSource : IVote
    {
        private List<VoteInfo> _votes; 
        public MockVoteSource()
        {
            _votes = new List<VoteInfo>();
        }

        public bool PostVote(VoteInfo Vote)
        {
            _votes.Add(Vote);
            return true;
        }
    }
}
