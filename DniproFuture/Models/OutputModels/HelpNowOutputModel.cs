﻿using System;
using System.Collections.Generic;

namespace DniproFuture.Models.OutputModels
{
    public class NeedHelpOutputModel
    {
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> Photos { get; set; }
        public double Summ { get; set; }
        public double NeedSum { get; set; }
        public int Age { get; set; }
        public DateTime Birthday { get; set; }
        public string About { get; set; }
        public int Id { get; set; }
    }
}
