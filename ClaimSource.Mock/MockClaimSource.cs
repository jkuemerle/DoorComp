using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

using DoorComp.Common;

namespace ClaimSource.Mock
{
    [Export(typeof(IClaimSource))]
    public class MockClaimSource : IClaimSource
    {
        private Dictionary<string, ClaimInfo> _claims;
 
        public MockClaimSource()
        {
            _claims = new Dictionary<string, ClaimInfo>();
        }

        public bool Claim(string DoorID, ClaimInfo Claim)
        {
            if (!_claims.ContainsKey(DoorID))
                _claims.Add(DoorID, Claim);
            else
                _claims[DoorID] = Claim;
            return true;
        }

        public ClaimInfo GetClaim(string DoorID)
        {
            if (_claims.ContainsKey(DoorID))
                return _claims[DoorID];
            return null;
        }
    }
}
