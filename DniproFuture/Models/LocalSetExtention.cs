using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DniproFuture.Models
{
    public static class NeedHelpLocalSetExtention
    {
        public static string GetFullName(this NeedHelpLocalSet local)
        {
            return string.Format("{0} {1}", local.FirstName, local.LastName);
        }
    }
}