using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class StringExtensions
    {
        public static int ToInt(this object obj)
        {
            try
            {
                return Convert.ToInt32(obj);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public static decimal ToDecimal(this object obj)
        {
            try
            {
                return Convert.ToDecimal(obj);
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public static string EncodeToBase64(this string obj)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(obj);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string DecodeToBase64(this string obj)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(obj);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
