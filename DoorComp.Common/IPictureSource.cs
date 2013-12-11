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

        public string URL { get; set; }

        public string Title { get; set; }

    }

    public interface IPictureSource
    {
        IList<PictureInfo> ListPictures(string SearchString);

        PictureInfo GetPicture(string ID);       
    }

}
