using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DoorComp.Common;

namespace DoorSource.SQLite
{
    public class DoorSource : IDoorSource
    {
        public DoorInfo GetDoor(string DoorID)
        {
            return new DoorInfo() { Description = "foo" };
        }
    }
}
