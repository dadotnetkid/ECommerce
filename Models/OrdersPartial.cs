using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public partial class Orders
    {
        public string Status => OrderStatus.OrderByDescending(x=>x.Id).FirstOrDefault()?.Status;
    }
}
