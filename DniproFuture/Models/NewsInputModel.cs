using System.Collections.Generic;

namespace DniproFuture.Models
{
    public class NewsInputModel
    {
        public News NewsInfo { get; set; }
        public List<NewsLocal> Locals { get; set; }
    }
}