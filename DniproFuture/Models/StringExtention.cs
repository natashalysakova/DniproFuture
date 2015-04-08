using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace DniproFuture.Models
{
    public static class StringExtention
    {
        public static string GetClearTitle(this string title)
        {
            Regex pattern = new Regex("[.,!?:;-]|[\"]");

            string s = pattern.Replace(title, " ");
            pattern = new Regex("[(\\s+)]");
            s = pattern.Replace(s, "-");

            return s;
        }
    }
}