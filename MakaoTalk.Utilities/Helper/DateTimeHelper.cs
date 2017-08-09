using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaoTalk.Utilities.Helper
{
    public static class DateTimeHelper
    {
        public static string ToString24Hour(this DateTime src)
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ToString24HourNumeric(this DateTime src)
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        public static string ToYYYYMMDD(this DateTime src)
        {
            return src.ToString("yyyyMMdd");
        }

        public static string ToYYMMDD(this DateTime src)
        {
            return src.ToString("yyMMdd");
        }
    }
}
