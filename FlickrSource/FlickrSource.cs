using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

using FlickrNet;
using DoorComp.Common;

namespace FlickrSource
{
    [Export(typeof(IPictureSource))]
    public class FlickrSource : IPictureSource
    {

        private Flickr _flickr;

        public bool RequiresCredentials
        {
            get
            {
                return true;
            }
        }

        public bool Init(PictureCredentials Credentials)
        {
            _flickr = new Flickr(Credentials.APIKey, Credentials.Secret);
            return true;
        }
        public FlickrSource() { }

        public FlickrSource(string APIKey, string Secret)
        {
            _flickr = new Flickr(APIKey, Secret);
        }

        public IList<PictureInfo> ListPictures(string SearchString)
        {
            var retVal = new List<PictureInfo>();
            PhotoSearchOptions options = new PhotoSearchOptions() { Tags = SearchString, TagMode= TagMode.AllTags };
            retVal = (from a in _flickr.PhotosSearch(options) select new PictureInfo() { ID = a.PhotoId, FullSizeURL = a.MediumUrl, 
                ThumbnailURL = a.ThumbnailUrl, Title = a.Title, URL = a.WebUrl, MediumURL = a.MediumUrl }).ToList();
            return retVal;
        }

        public PictureInfo GetPicture(string ID)
        {
            var retVal = new PictureInfo();
            var pic = _flickr.PhotosGetInfo(ID);
            if(null != pic)
            {
                retVal.ID = pic.PhotoId;
                retVal.FullSizeURL = pic.MediumUrl;
                retVal.ThumbnailURL = pic.ThumbnailUrl;
                retVal.Title = pic.Title;
                retVal.URL = pic.WebUrl;
            }
            return retVal;
        }
    }
}
