using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Helpers;

namespace Web.Controllers
{
    public class ControllerActionHelper
    {

        public IEnumerable<string> ControllerNames()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            var controlleractionlist = asm.GetTypes()
                .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                .SelectMany(type =>
                    type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Where(m => m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute),
                    true).Any())
                .Select(x => new { Controller = x.DeclaringType.Name, Action = x.Name, ReturnType = x.ReturnType.Name, Attributes = x.GetCustomAttributes() });



            return controlleractionlist.Select(x => x.ToString()).ToList();
        }
        public IEnumerable<string> ActionNames()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            var controlleractionlist = asm.GetTypes()
                .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Where(m => m.GetCustomAttributes(typeof(OnUserAuthorizationAttribute), true).Any())
                .Select(x => new { Controller = x.DeclaringType.Name, Action = x.Name, ReturnType = x.ReturnType.Name, Attributes = x.GetCustomAttributes() })
                .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();

            //var res = controlleractionlist.Where(x => x.Controller == controllerName);
            List<string> action = new List<string>();
            foreach (var i in controlleractionlist)
            {
                foreach (var x in i.Attributes)
                {
                    var authAttr = x as OnUserAuthorizationAttribute;
                    var a = authAttr?.ActionName;
                    if (a != null)
                        action.Add(a);
                }


            }
            return action;
        }
        public IEnumerable<string> GetActionNames(string controllerName)
        {
            Dictionary<string, string> _dict = new Dictionary<string, string>();
            List<string> str = new List<string>();


            Assembly asm = Assembly.GetExecutingAssembly();
            var controlleractionlist = asm.GetTypes()
                .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                .SelectMany(type =>
                    type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute),
                    true).Any()).ToList();






            var attrs = controlleractionlist.FirstOrDefault().GetCustomAttributes(true);
            /*foreach (object attr in attrs)
            {

                if (authAttr != null)
                {
                    //            string propName = prop.Name;
                    string auth = authAttr.ActionName;

                    str.Add(auth);
                }

            }*/
            return str;
        }
    }
}
/*
                 Assembly asm = Assembly.GetExecutingAssembly();
            var controlleractionlist = asm.GetTypes()
                .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                .Select(x => new { Controller = x.DeclaringType.Name, Action = x.Name, ReturnType = x.ReturnType.Name, Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))) })
                .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();
            var res = controlleractionlist.Select(x => x.Controller);*/
