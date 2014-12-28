using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using FlickrSource;
using NUnit.Framework;

namespace FlickrSource.Tests
{
    [TestFixture]
    public class Tests
    {
        private FlickrSource src;

        [TestFixtureSetUp]
        public void Setup()
        {
            var creds = File.ReadAllLines(@"c:\temp\flickrcred.txt");
            if(creds.Length > 1)
            {
                src = new FlickrSource(creds[0], creds[1]);
            }
        }

        [Test]
        public void TestListDoorComp()
        {
            var pics = src.ListPictures("doorcomp,codemash","");
            Assert.Greater(pics.Count, 0);
        }


    }
}
