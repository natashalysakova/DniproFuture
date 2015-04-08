using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DniproFuture.Models
{
    public static class DateTimeExtention
    {
        public static int GetAge(this DateTime birthday)
        {
            var age = DateTime.Today.Year - birthday.Year;
            var monthdiff = DateTime.Today.Month - birthday.Month;
            var daydiff = DateTime.Today.Day - birthday.Day;
            if (daydiff < 0)
            {
                monthdiff--;
            }
            if (monthdiff < 0)
            {
                age--;
            }
            return age;
        }
    }
}