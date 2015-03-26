using System.Collections.Generic;

namespace DniproFuture.Models
{
    public class NeedHelpInputModel
    {
        public NeedHelp WhatNeed { get; set; }
        public List<NeedHelpLocal> WhoNeed { get; set; }
    }
}