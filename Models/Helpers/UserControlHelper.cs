using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using DevExpress.XtraEditors;

namespace Helpers
{
    public static class UserControlHelper
    {
        public static void ShowUserControl(this PanelControl pnl, XtraUserControl type )
        {
            //if(pnl.Controls[type.] == type)
            if (pnl.Controls[type.Name] is XtraUserControl userControl)
            {
                type.BringToFront();
            }
            else
            {
                pnl.Controls.Add(type);
                type.BringToFront();
            }

        }
    }
}
