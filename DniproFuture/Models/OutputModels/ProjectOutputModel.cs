using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DniproFuture.Models.OutputModels
{
    public class ProjectOutputModel
    {
        public string Title { get; set; }
        public string About { get; set; }
        public List<string> Photos { get; set; }
        public double Summ { get; set; }
        public double NeedSum { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public bool IsDone { get; set; }
    }
}