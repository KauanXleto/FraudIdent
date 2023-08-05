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
        public List<LobInfo> GetLastLobInfo()
        {
            return this.coreBusinessRules.GetLobInfo();
        }

        [HttpPost("saveConfigurations")]
        public void SaveConfigurations([FromBody]string obj)
        {
            var entity = JsonConvert.DeserializeObject<ModelUserConfiguration>(obj);

            if (!entity.ManualDataInsert)
            {
                if(entity.TruckId > 0)
                    entity.Truck.Id = entity.TruckId.Value;
            }
            else
            {
                entity.Truck.Id = 0;
            }

            this.coreBusinessRules.SaveTruck(entity.Truck);
            this.coreBusinessRules.SaveBalanceInfo(entity.BalanceInfo);
        }
    }
}