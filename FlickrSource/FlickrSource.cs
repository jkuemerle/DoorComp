using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FlickrNet;
using IPictureSource;

namespace FlickrSource
{
    public class FlickrSource : IPictureSource.IPictureSource
    {

        private Flickr _flickr;
        private FlickrSource() { }

        public FlickrSource(string APIKey, string Secret)
        {
            _flickr = new Flickr(APIKey, Secret);
        }

        public IList<PictureInfo> ListPictures(string SearchString)
        {
            var retVal = new List<PictureInfo>();
            PhotoSearchOptions options = new PhotoSearchOptions() { Tags = SearchString, UserId = "56659902@N04" };
            retVal = (from a in _flickr.PhotosSearch(options) select new PictureInfo() { ID = a.PhotoId, FullSizeURL = a.MediumUrl, ThumbnailURL = a.ThumbnailUrl}).ToList();
            return retVal;
        }
    }
}
