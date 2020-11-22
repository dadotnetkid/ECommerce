using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public class UcSettings
    {
        public object DataSource { get; set; }

        public List<Columns> Columns { get; set; }

    }

    public class Columns
    {
        public string ColumnName { get; set; }
        public string DataPropertyName { get; set; }
    }
}
