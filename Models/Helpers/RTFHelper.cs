using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.Export;
using DevExpress.XtraRichEdit.Export.Html;

namespace Helpers
{
    public static class RTFHelper
    {
        public static string ToHtml(this RichEditControl richEditControl)
        {
            HtmlDocumentExporterOptions options = new HtmlDocumentExporterOptions();
            options.ExportRootTag = ExportRootTag.Body;
            options.CssPropertiesExportType = CssPropertiesExportType.Inline;
            HtmlExporter exporter = new HtmlExporter(richEditControl.Model, options);
            string stringHtml = exporter.Export();
            return stringHtml;
        }

        public static string HtmlToRtf(this RichEditControl richEditControl,string markup)
        {
            HtmlDocumentExporterOptions options = new HtmlDocumentExporterOptions();
            options.ExportRootTag = ExportRootTag.Body;
            options.CssPropertiesExportType = CssPropertiesExportType.Inline;
            HtmlExporter exporter = new HtmlExporter(richEditControl.Model, options);
            string stringHtml = exporter.Export();
            return stringHtml;
        }
        public static void ToBold(this RichEditControl richEditControl)
        {
            var txt = richEditControl.Document;
            DocumentRange res = txt.Selection;
            CharacterProperties cp = txt.BeginUpdateCharacters(res);
            cp.Bold = cp.Bold != true;
            txt.EndUpdateCharacters(cp);
        }
        public static void ToItalic(this RichEditControl richEditControl)
        {
            var txt = richEditControl.Document;
            DocumentRange res = txt.Selection;
            CharacterProperties cp = txt.BeginUpdateCharacters(res);
            cp.Italic = cp.Italic != true;

            txt.EndUpdateCharacters(cp);
        }
        public static void ToUnderline(this RichEditControl richEditControl)
        {
            var txt = richEditControl.Document;


            DocumentRange res = txt.Selection;
            CharacterProperties cp = txt.BeginUpdateCharacters(res);
            if (cp.Underline == UnderlineType.None)
            {
                cp.Underline = UnderlineType.Single;
            }
            else
            {
                cp.Underline = UnderlineType.None;
            }
            txt.EndUpdateCharacters(cp);
        }
        public static void IncreaseFont(this RichEditControl richEditControl)
        {
            var txt = richEditControl.Document;


            DocumentRange res = txt.Selection;
            CharacterProperties cp = txt.BeginUpdateCharacters(res);
            cp.FontSize += 2.0f;
            txt.EndUpdateCharacters(cp);
        }
        public static void DecreaseFont(this RichEditControl richEditControl)
        {
            var txt = richEditControl.Document;


            DocumentRange res = txt.Selection;
            CharacterProperties cp = txt.BeginUpdateCharacters(res);
            cp.FontSize -= 2.0f;
            txt.EndUpdateCharacters(cp);
        }
        public static void ChangeFont(this RichEditControl richEditControl, string fontName)
        {
            var txt = richEditControl.Document;


            DocumentRange res = txt.Selection;
            CharacterProperties cp = txt.BeginUpdateCharacters(res);
            cp.FontName = fontName;
            txt.EndUpdateCharacters(cp);
        }

        public static void DefaultFont(this RichEditControl richEditControl)
        {
            var txt = richEditControl.Document;
            txt.BeginUpdate();
            txt.DefaultCharacterProperties.FontSize = 12f;
            txt.EndUpdate();
        }
    }
}
