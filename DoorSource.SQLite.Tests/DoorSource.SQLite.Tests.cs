using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using NUnit.Framework;

using DoorComp.Common;

using DoorSource.SQLite;

namespace DoorSource.SQLite.Tests
{
    [TestFixture]
    public class DoorSourceTests
    {
        [TestCase("8368884461")]
        [Test]
        public void TestGetDoor(string DoorID)
        {
            var test = new DoorSource().GetDoor(DoorID);
            Assert.IsNotNull(test);
            Assert.IsNotEmpty(test.Location);
        }
    }
}
