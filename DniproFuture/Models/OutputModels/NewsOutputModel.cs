using System;
using System.Collections.Generic;

namespace DniproFuture.Models.OutputModels
{
    public class NewsOutputModel
    {
        public string Title { get; set; }
        public string ShortText { get; set; }
        public List<string> Photo { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public int Id { get; set; }
    }
}