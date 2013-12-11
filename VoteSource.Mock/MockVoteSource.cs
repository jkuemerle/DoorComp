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
        public bool PostVote(VoteInfo Vote)
        {
            return true;
        }
    }
}
