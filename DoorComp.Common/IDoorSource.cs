using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorComp.Common
{
    public class DoorInfo
    {
        public string Description { get; set; }
    }
    public interface IDoorSource
    {
        DoorInfo GetDoor(string DoorID);

    }
}
