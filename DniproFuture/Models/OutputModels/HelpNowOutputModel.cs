using System;
using System.Collections.Generic;

namespace DniproFuture.Models.OutputModels
{
    public class HelpNowOutputModel
    {
        public string FullName { get; set; }
        public List<string> Photos { get; set; }
        public double Summ { get; set; }
        public double NeedSum { get; set; }
        public int Age { get; set; }
        public DateTime Birthday { get; set; }
        public string About { get; set; }
        public int Id { get; set; }
    }
}
