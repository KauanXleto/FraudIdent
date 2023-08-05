using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FraudIdent.Backbone.BusinessEntities
{
    public class Truck
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TruckParam TruckParam { get; set; }
    }
}
