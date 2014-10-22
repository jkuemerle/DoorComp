using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;

using FlickrNet;
using DoorComp.Common;

namespace FlickrSource
{
    public static class FlickrSourceExtensions
    {
        public static string RemoveProtocol(this string val)
        {
            var ret = val.Replace("http://", "https://");
            //ret = ret.Replace("https://", "//");
            return ret;
        }

    }

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
            _flickr.InstanceCacheDisabled = true;
            return true;
        }
        public FlickrSource() { }

        public FlickrSource(string APIKey, string Secret)
        {
            _flickr = new Flickr(APIKey, Secret);
            _flickr.InstanceCacheDisabled = true;
        }

        public IList<PictureInfo> ListPictures(string SearchString)
        {
            var retVal = new List<PictureInfo>();
            PhotoSearchOptions options = new PhotoSearchOptions() { Tags = SearchString, TagMode= TagMode.AllTags };
            retVal = (from a in _flickr.PhotosSearch(options) select new PictureInfo() { ID = a.PhotoId, 
                FullSizeURL = a.MediumUrl.RemoveProtocol(), ThumbnailURL = a.ThumbnailUrl.RemoveProtocol(), 
                Title = a.Title, URL = a.WebUrl.RemoveProtocol(), MediumURL = a.MediumUrl.RemoveProtocol() }).ToList();
            return retVal;
        }

        public PictureInfo GetPicture(string ID)
        {
            var retVal = new PictureInfo();
            var pic = _flickr.PhotosGetInfo(ID);
            if(null != pic)
            {
                retVal.ID = pic.PhotoId;
                retVal.FullSizeURL = pic.MediumUrl.RemoveProtocol();
                retVal.ThumbnailURL = pic.ThumbnailUrl.RemoveProtocol();
                retVal.Title = pic.Title;
                retVal.URL = pic.WebUrl.RemoveProtocol();
                retVal.MediumURL = pic.MediumUrl.RemoveProtocol();
            }
            return retVal;
        }
    }
}
