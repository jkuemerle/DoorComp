using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorComp.Common
{
    public class PictureInfo
    {
        public string ID { get; set; }

        public string FullSizeURL { get; set; }

        public string ThumbnailURL { get; set; }

        public string MediumURL { get; set; }

        public string URL { get; set; }

        public string Title { get; set; }

    }

    public class PictureCredentials
    {
        public string APIKey { get; set; }
        public string Secret { get; set; }

        public PictureCredentials(params string[] Arguements)
        {
            if (Arguements.Length > 0)
                this.APIKey = Arguements[0];
            if (Arguements.Length > 1)
                this.Secret = Arguements[1];
        }
    }

    public interface IPictureSource
    {
        IList<PictureInfo> ListPictures(string SearchString, string EventTag);

        PictureInfo GetPicture(string ID);

        bool RequiresCredentials { get; }

        bool Init(PictureCredentials Credentials);
    }

}
