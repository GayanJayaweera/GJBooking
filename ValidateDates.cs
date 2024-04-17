using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJBooking
{
    internal class ValiDate
    {
       public static DateTime? Vdatetime(string dateTimeIn)
        {
            DateTime dateTimeOut;
            if (DateTime.TryParse(dateTimeIn, out dateTimeOut))
            {
                return dateTimeOut;
            }
            else
            {
                return null;
            }
        }
    }
}
