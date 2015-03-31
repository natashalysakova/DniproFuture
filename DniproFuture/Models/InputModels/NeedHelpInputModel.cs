using System.Collections.Generic;

namespace DniproFuture.Models.InputModels
{
    public class NeedHelpInputModel
    {
        public NeedHelp WhatNeed { get; set; }
        public List<NeedHelpLocalSet> WhoNeed { get; set; }
    }
}