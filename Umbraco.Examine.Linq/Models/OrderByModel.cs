using Remotion.Linq.Clauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.Examine.Linq.Models
{
    public class OrderByModel
    {
        public OrderingDirection Direction { get; set; }
        public string Field { get; set; }

        public OrderByModel(OrderingDirection direction, string field)
        {
            this.Direction = direction;
            this.Field = field;
        }
    }
}
