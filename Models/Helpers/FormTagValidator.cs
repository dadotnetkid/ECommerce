using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;

namespace Helpers
{
    public static class FormHelper
    {
        static DXErrorProvider dXErrorProvider = new DXErrorProvider();
        public static bool ValidateForm(this XtraForm frm)
        {

            dXErrorProvider.ClearErrors();

            foreach (Control c in frm.Controls.Cast<Control>().Where(x => x.Tag != null).OrderBy(x => x.TabIndex))
            {
                if (c is BaseEdit ctrl)
                    if (!string.IsNullOrEmpty(ctrl.Tag?.ToString()))
                    {
                        if (ctrl.EditValue == null)
                        {
                            dXErrorProvider.SetError(ctrl, ctrl.Tag?.ToString());
                            dXErrorProvider.SetIconAlignment(ctrl, ErrorIconAlignment.MiddleRight);
                            ctrl.Focus();
                            return false;
                        }

                    }
            }

            return true;
        }

        public static XtraForm ShowDialogBox(this XtraForm frm)
        {
            frm.ShowDialog();
            return frm;
        }
    }
}
