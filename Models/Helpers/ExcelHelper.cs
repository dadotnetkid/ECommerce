using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web;
using DevExpress.Web;

namespace Helpers
{
    public static class DevexpressHelper
    {
        public static string  SaveFile(this UploadedFile uploadedFile)
        {
            var extensionName = Path.GetExtension(uploadedFile.FileName);
            string filename = "";
            if (extensionName.ToLower().Contains("xlsx"))
                filename = HttpContext.Current.Server.MapPath(System.IO.Path.Combine("~/content/excel", Guid.NewGuid().ToString() + ".xlsx"));
            else
                filename = HttpContext.Current.Server.MapPath(System.IO.Path.Combine("~/content/excel", Guid.NewGuid().ToString() + ".xls"));
            uploadedFile.SaveAs(filename);
            return filename;
        }
    }
    public class ExcelHelper
    {
        public ExcelHelper(string filename)
        {
            var extensionName = Path.GetExtension(filename);
            if (extensionName.ToLower().Contains("xlsx"))
                connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filename};Extended Properties='Excel 12.0 Xml;HDR=Yes;IMEX=1'"; 
            else
                connectionString = $@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={filename};Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'";
        }


        public string ConnectionString { get { return connectionString; } set { connectionString = value; } }
        private string connectionString;
        public DataTable ExecuteReader()
        {
            DataTable dt = new DataTable();
            using (OleDbConnection cn = new OleDbConnection(ConnectionString))
            {
                cn.Open();
                string sheetname = cn.GetSchema("Tables").Rows[0]["TABLE_NAME"].ToString();
                var query = $"select * from [{sheetname}]";
                using (OleDbCommand cmd = new OleDbCommand(query, cn))
                {
                    OleDbDataAdapter da = new OleDbDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
            }
            return dt;
        }
        public DataTable ExecuteReader(string query)
        {
            DataTable dt = new DataTable();
            using (OleDbConnection cn = new OleDbConnection(ConnectionString))
            {
                cn.Open();
                using (OleDbCommand cmd = new OleDbCommand(query, cn))
                {
                    OleDbDataAdapter da = new OleDbDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
            }
            return dt;
        }
    }
}