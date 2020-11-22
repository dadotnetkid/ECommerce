using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public partial class Signatories
    {
        /// public List<int> SelectedBACMembers => this.BACMembers.Select(x => x.Id).ToList();
        [Newtonsoft.Json.JsonIgnoreAttribute]
        public bool isSelected { get; set; }
    }
}
