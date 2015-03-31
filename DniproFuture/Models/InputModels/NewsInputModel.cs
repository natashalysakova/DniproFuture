using System.Collections.Generic;

namespace DniproFuture.Models.InputModels
{
    public class NewsInputModel
    {
        public News NewsInfo { get; set; }
        public List<NewsLocal> Locals { get; set; }
    }
}