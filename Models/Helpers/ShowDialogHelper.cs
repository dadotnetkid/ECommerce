using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Helpers
{
    public static class ShowDialogHelper
    {
        public static TEntity ShowDialogResult<TEntity>(this TEntity form)
        where TEntity : Form
        {
            (form as Form)?.ShowDialog();
            return form;
        }
    }
}
