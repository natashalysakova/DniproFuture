using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DniproFuture.Models.InputModels
{
    public class PartnersInputModel
    {
        public Partners MainInfo { get; set; }
        public PartnersLocal[] LocalInfo { get; set; }
    }
}