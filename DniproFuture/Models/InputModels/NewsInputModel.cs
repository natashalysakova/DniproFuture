using System.Collections.Generic;

namespace DniproFuture.Models.InputModels
{
    public class NewsInputModel
    {
        public News NewsInfo { get; set; }
        public List<NewsLocalSet> Locals { get; set; }
    }
}