using FraudIdent.Backbone.BusinessEntities;

namespace FraudIdent.Models
{
    public class SaveLobInfoRequest
    {            
        public int? TruckId { get; set; }
        public bool? HasError { get; set; }
        public bool? HasSuccess { get; set; }
        public bool? IsDistanceImage { get; set; }
        public string? MessageError { get; set; }
        public string? BackImageTruck { get; set; }
        public string? SideImageTruck { get; set; }

    }
}
