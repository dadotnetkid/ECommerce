using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Helpers
{
    public static class AnonymousHelper
    {
        public static ExpandoObject ToExpando(this object anonymousObject)
        {
            IDictionary<string, object> anonymousDictionary = new RouteValueDictionary(anonymousObject);
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var item in anonymousDictionary)
                expando.Add(item);



            return (ExpandoObject) expando;
        }
        public static List<ExpandoObject> ToExpandoList<T>(this IEnumerable<T> ie)
        {
            return ie.Select(o => o.ToExpando()).ToList();
        }
    }
}
