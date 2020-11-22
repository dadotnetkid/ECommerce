using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class UrlHelper
    {
        public static string ReplaceURL(this string str)
        {
            var chr = "!@#$%^&*()_+<>?/.,:';{}[]";
            foreach (var i in chr)
            {
                str = str.Replace(i.ToString(), "");
            }
            return str;
        }
    }
}
