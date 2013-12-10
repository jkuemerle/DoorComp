using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPictureSource
{
    public class PictureInfo
    {
        public string ID { get; set; }
        public string FullSizeURL { get; set; }

        public string ThumbnailURL { get; set; }

    }
    public interface IPictureSource
    {
        IList<PictureInfo> ListPictures(string SearchString);
 
        
    }
}
