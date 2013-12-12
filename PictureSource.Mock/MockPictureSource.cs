using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DoorComp.Common;

namespace PictureSource.Mock
{
    public class MockPictureSource : IPictureSource
    {
        private Dictionary<string,PictureInfo> _pics;

        public MockPictureSource()
        {
            _pics = new Dictionary<string,PictureInfo>();
            _pics.Add("1",new PictureInfo() { ID = "1", Title = "Foo", FullSizeURL = "http://evilco.com/pic.jpg", ThumbnailURL = "http://evilco.com/pic.jpg", URL = "http://evilco.com" });
            _pics.Add("2",new PictureInfo() { ID = "2", Title = "Bar", FullSizeURL = "http://evilco.com/pic.jpg", ThumbnailURL = "http://evilco.com/pic.jpg", URL = "http://evilco.com" });
            _pics.Add("3",new PictureInfo() { ID = "3", Title = "Baz", FullSizeURL = "http://evilco.com/pic.jpg", ThumbnailURL = "http://evilco.com/pic.jpg", URL = "http://evilco.com" });
        }
        public IList<PictureInfo> ListPictures(string SearchString)
        {
            return (from a in _pics select a.Value).ToList();
        }

        public PictureInfo GetPicture(string ID)
        {
            if (_pics.ContainsKey(ID))
                return _pics[ID];
            else
                return null;
        }
    }
}
