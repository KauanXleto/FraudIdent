using FraudIdent.Backbone.BusinessEntities;
using System.Diagnostics;

namespace FraudIdent.Models
{
    public class ModelUserConfiguration
    {
        public bool ManualDataInsert { get; set; }
        public int? TruckId { get; set; }
        public Truck Truck { get; set; }
        public BalanceInfo BalanceInfo { get; set; }
    }
}
