using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Models
{
    public partial class UserRoles : IRole<string>
    {
        //    public string Priviledges => string.Join(", ", this.UserRolesInActions.Select(x => x.Action));

        private string actions;

        public string Actions
        {
            get
            {

                if (string.IsNullOrEmpty(actions))
                    actions = string.Join(", ", this.Functions.Select(x => x.Action));
                return actions;
            }
            set { actions = value; }
        }

        private string _ids;
        public string Ids
        {
            get
            {

                if (string.IsNullOrEmpty(actions))
                    _ids = string.Join(", ", this.Functions.Select(x => x.Id));
                return _ids;
            }
            set { _ids = value; }
        }

    }
}
