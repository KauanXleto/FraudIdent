using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FraudIdent.Backbone.BusinessEntities
{
    public class LobInfo
    {
        public int Id { get; set; }
        public int? TruckId { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? HasError { get; set; }
        public bool? HasSuccess { get; set; }
        public string? MessageError { get; set; }
        public string? BackImageTruck { get; set; }
        public string? SideImageTruck { get; set; }

        public Truck Truck { get; set; }
    }
}
