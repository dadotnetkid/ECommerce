using DevExpress.XtraPrinting;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Helpers
{
    public class CustomPrintPreviewCommand : PrintPreviewCommand
    {
        public CustomPrintPreviewCommand(IRichEditControl control)
            : base(control)
        {

        }

        public override void ForceExecute(DevExpress.Utils.Commands.ICommandUIState state)
        {
            base.ForceExecute(state);
        }

        protected override void ExecuteCore()
        {
            PrintingSystem printSystem = new PrintingSystem();
            PrintableComponentLink printLink = new PrintableComponentLink(printSystem);

            printLink.Component = (base.Control as RichEditControl);
            printLink.CreateDocument();

            //Handle when the preview form is closed  
            printSystem.PreviewFormEx.FormClosed += PreviewFormEx_FormClosed;

            //Show the preview  
            printLink.ShowPreview();
        }

        private void PreviewFormEx_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }
    }
}
