using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using DevExpress.XtraPrinting.Native;

namespace Helpers
{
    public static class GridViewHelpers
    {
        public static GridViewSettings AddAddEditDeleteToolbar(this GridViewSettings settings)
        {
            settings.SettingsBehavior.AllowFocusedRow = true;
            settings.CommandColumn.ShowEditButton = false;
            settings.CommandColumn.ShowNewButtonInHeader = false;
            settings.CommandColumn.ShowDeleteButton = false;
            settings.CommandColumn.Visible = false;
            settings.SettingsBehavior.AllowFocusedRow = true;
            settings.Toolbars.Add(tb =>
            {
                tb.Enabled = true;
                tb.Position = GridToolbarPosition.Top;


                tb.Items.Add(GridViewToolbarCommand.New);
                tb.Items.Add(GridViewToolbarCommand.Edit);
                tb.Items.Add(GridViewToolbarCommand.Delete);
            });

            return settings;
        }
        public static GridViewSettings AddEditDeleteImportToolbar(this GridViewSettings settings, Action<MVCxGridViewToolbarItem> items)
        {
            settings.SettingsBehavior.AllowFocusedRow = true;
            settings.CommandColumn.ShowEditButton = false;
            settings.CommandColumn.ShowNewButtonInHeader = false;
            settings.CommandColumn.ShowDeleteButton = false;
            settings.CommandColumn.Visible = false;
            settings.SettingsBehavior.AllowFocusedRow = true;
            settings.Toolbars.Add(tb =>
            {
                tb.Enabled = true;
                tb.Position = GridToolbarPosition.Top;


                tb.Items.Add(GridViewToolbarCommand.New);
                tb.Items.Add(GridViewToolbarCommand.Edit);
                tb.Items.Add(GridViewToolbarCommand.Delete);
                

                tb.Items.Add(items);
            });

            return settings;
        }
        public static void AddAddEditDeleteToolbar(this GridViewSettings settings, MVCxGridViewToolbarItemCollection items)
        {
            settings.SettingsBehavior.AllowFocusedRow = true;
            settings.CommandColumn.ShowEditButton = false;
            settings.CommandColumn.ShowNewButtonInHeader = false;
            settings.CommandColumn.ShowDeleteButton = false;
            settings.Toolbars.Add(tb =>
            {
                tb.Enabled = true;
                tb.Position = GridToolbarPosition.Top;


                tb.Items.Add(GridViewToolbarCommand.New);
                tb.Items.Add(GridViewToolbarCommand.Edit);
                tb.Items.Add(GridViewToolbarCommand.Delete);


                tb.Items.AddRange(items);

            });
        }
    }
}
