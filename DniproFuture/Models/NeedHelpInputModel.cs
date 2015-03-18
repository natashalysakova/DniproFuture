using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace DniproFuture.Models
{
    public class NeedHelpInputModel
    {
        public NeedHelp WhatNeed { get; set; }

        public List<NeedHelpLocal> WhoNeed { get; set; }

    }

}