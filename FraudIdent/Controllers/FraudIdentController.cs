using FraudIdent.Backbone.BusinessEntities;
using FraudIdent.Backbone.BusinessRules;
using FraudIdent.Backbone.Providers;
using FraudIdent.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FraudIdent.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FraudIdentController : ControllerBase
    {
        //private readonly ILogger<FraudIdentController> _logger;

        CoreBusinessRules coreBusinessRules { get; set; }
        CoreSQLProvider coreSQLProvider { get; set; }

        public FraudIdentController(CoreBusinessRules rules,
                                         CoreSQLProvider provider)
        {
            //_logger = logger;
            coreBusinessRules = rules;
            coreSQLProvider = provider;
        }

        [HttpGet("getTrucks")]
        public IEnumerable<Truck> GetTucks()
        {
            return this.coreBusinessRules.GetTrucks();
        }

        [HttpGet("getBalanceInfos")]
        public BalanceInfo GetBalanceInfo()
        {
            return this.coreBusinessRules.GetBalanceInfo();
        }

        [HttpGet("getLastLobInfo")]
        public List<LobInfo> GetLastLobInfo([FromQuery] bool? IsDistanceImage, [FromQuery] int? TruckId)
        {
            return this.coreBusinessRules.GetLobInfo(new LobInfo() { IsDistanceImage = IsDistanceImage, TruckId = TruckId });
        }

        [HttpPost("saveConfigurations")]
        public void SaveConfigurations([FromBody] ModelUserConfiguration entity)
        {
            //var entity = JsonConvert.DeserializeObject<ModelUserConfiguration>(obj);

            if (!entity.ManualDataInsert)
            {
                if (entity.TruckId > 0)
                    entity.Truck.Id = entity.TruckId.Value;
            }
            else
            {
                entity.Truck.Id = 0;
            }

            this.coreBusinessRules.SaveTruck(entity.Truck);
            this.coreBusinessRules.SaveBalanceInfo(entity.BalanceInfo);
        }


        [HttpPost("saveLobInfo")]
        public void SaveLobInfo([FromBody] SaveLobInfoRequest entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.SideImageTruck) && !string.IsNullOrWhiteSpace(entity.BackImageTruck))
            {
                var lobInfo = new LobInfo();

                lobInfo.TruckId = (entity.TruckId > 0 ? entity.TruckId : 1);
                lobInfo.HasError = entity.HasError;
                lobInfo.HasSuccess = entity.HasSuccess;
                lobInfo.IsDistanceImage = entity.IsDistanceImage;

                lobInfo.MessageError = entity.MessageError;

                var sideMimetype = this.coreBusinessRules.GetMimeType(entity.SideImageTruck);
                var backMimetype = this.coreBusinessRules.GetMimeType(entity.BackImageTruck);

                lobInfo.SideImageTruck = $"data:{sideMimetype};base64," + entity.SideImageTruck;
                lobInfo.BackImageTruck = $"data:{backMimetype};base64," + entity.BackImageTruck;

                this.coreBusinessRules.SaveLobInfo(lobInfo);
            }
        }

        [HttpGet("getAllConfig")]
        public dynamic GetAllConfig([FromQuery] int? TruckId)
        {

            if (TruckId == null || (TruckId != null && TruckId == 0))
            {
                TruckId = 1;
            }

            var result = this.coreBusinessRules.GetAllConfig(TruckId.Value);

            return result;
        }

        [HttpPost("changeTruckSelected")]
        public void ChangeTruckSelected([FromBody] int? TruckId)
        {
            if (TruckId == null || (TruckId != null && TruckId == 0))
            {
                TruckId = 1;
            }

            this.coreBusinessRules.NotifyMqttChangeTruck(TruckId.Value);
        }
    }
}