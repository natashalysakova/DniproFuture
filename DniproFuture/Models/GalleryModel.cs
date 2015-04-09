using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DniproFuture.Models
{
    public class GalleryModel
    {
        public List<string> Photos { get; set; }
        public GalleryType Type { get; set; }
    }

    public enum GalleryType
    {
        News,
        NeedHelp,
        Projects
    }
}