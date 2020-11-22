using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Models.ViewModels
{
    public class OrderReportVM
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string UserId=>HttpContext.Current.User.Identity.GetUserId();
    }
}
