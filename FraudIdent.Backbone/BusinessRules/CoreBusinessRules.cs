using FraudIdent.Backbone.BusinessEntities;
using FraudIdent.Backbone.Providers;
using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FraudIdent.Backbone.BusinessRules
{
    public class CoreBusinessRules
    {
        private IConfiguration _config;

        private readonly MqttClientRequest mqttClient;
        CoreSQLProvider CoreSqlProvider { get; set; }

        string _topicsToSubscribe { get; set; }

        static bool alreadyCreateSubscribs { get; set; }

        public CoreBusinessRules(IConfiguration _configuuration,
                                 CoreSQLProvider _CoreSqlProvider,
                                 MqttClientRequest _mqttClient,
                                 bool? onlyReference = false)
        {
            _config = _configuuration;
            this.CoreSqlProvider = _CoreSqlProvider;
            mqttClient = _mqttClient;

            if (onlyReference != true && !alreadyCreateSubscribs)
            {
                _topicsToSubscribe = _config.GetSection("MQTTConfig")["TopicsSubscribe"].ToString();

                SubscribeInTopics();
                alreadyCreateSubscribs = true;
            }
        }

        #region MQTT

        public async void SubscribeInTopics()
        {
            List<String> topics = _topicsToSubscribe.Split(";").ToList();

            // Connect to the MQTT broker
            var client = mqttClient.Connect();

            foreach (var topic in topics)
            {
                // Subscribe to a topic
                mqttClient.Subscribe(topic);
            }

            client.ApplicationMessageReceivedAsync += HandleMessageAsync;
        }

        async Task HandleMessageAsync(MqttApplicationMessageReceivedEventArgs e)
        {
            var _payLoad = e.ApplicationMessage.Payload;
            var payload = _payLoad != null ? Encoding.UTF8.GetString(_payLoad) : "";
            var topic = e.ApplicationMessage.Topic;

            var functionCallBackRequest = _config.GetSection("MQTTConfig")[topic];

            var _coreRules = new CoreBusinessRules(_config, this.CoreSqlProvider, mqttClient, true);

            MethodInfo methodInfo = typeof(CoreBusinessRules).GetMethod(functionCallBackRequest);

            // Create the parameter
            object[] parameters = new object[] { topic, payload };

            // Invoke the function
            try
            {
                methodInfo.Invoke(_coreRules, parameters);
            }
            catch (Exception ex)
            {
                mqttClient.Publish("applicationErrorMQtt", JsonConvert.SerializeObject(new { 
                    Topic = topic,
                    Message = payload,
                    Error = ex.ToString()
                }));
            }
        }

        #region MQTT Subscribe/Reponse
        public void GetBalanceInfoMQtt(string topic, string messag)
        {
            var balanceInfo = this.GetBalanceInfo();

            mqttClient.Publish($"response_{topic}", JsonConvert.SerializeObject(balanceInfo));
        }

        public void SaveBalanceInfoMQtt(string topic, string messag)
        {
            var balanceInfo = JsonConvert.DeserializeObject<BalanceInfo>(messag);

            this.SaveBalanceInfo(balanceInfo);

            mqttClient.Publish($"response_{topic}", JsonConvert.SerializeObject(balanceInfo));
        }
        public void GetTrucksMQtt(string topic, string messag)
        {
            List<Truck> list = new List<Truck>();

            if (!string.IsNullOrWhiteSpace(messag))
            {
                var truckFilter = JsonConvert.DeserializeObject<Truck>(messag);

                list = this.GetTrucks(truckFilter);
            }
            else
            {
                list = this.GetTrucks();
            }

            mqttClient.Publish($"response_{topic}", JsonConvert.SerializeObject(list));
        }

        public void SaveTrucksMQtt(string topic, string messag)
        {
            var truck = JsonConvert.DeserializeObject<Truck>(messag);

            this.SaveTruck(truck);
            mqttClient.Publish($"response_{topic}", JsonConvert.SerializeObject(truck));
        }

        public void GetTruckParamMQtt(string topic, string messag)
        {
            TruckParam entity = new TruckParam();

            if (!string.IsNullOrWhiteSpace(messag))
            {
                var truckParamId = JsonConvert.DeserializeObject<int>(messag);

                entity = this.GetTruckParam(truckParamId);
            }
            else
            {
                mqttClient.Publish($"response_{topic}", "Informe o id do caminhão ex: 'TruckId' = 1");
            }

            mqttClient.Publish($"response_{topic}", JsonConvert.SerializeObject(entity));
        }

        public void SaveTruckParamMQtt(string topic, string messag)
        {
            TruckParam entity = new TruckParam();

            if (!string.IsNullOrWhiteSpace(messag))
            {
                entity = JsonConvert.DeserializeObject<TruckParam>(messag);

                this.SaveTruckParam(entity);
            }
            else
            {
                mqttClient.Publish($"response_{topic}", "Passe o objeto TruckParam");
            }

            mqttClient.Publish($"response_{topic}", JsonConvert.SerializeObject(entity));
        }

        public void GetLobInfoMQtt(string topic, string messag)
        {
            List<LobInfo> entity = new List<LobInfo>();

            if (!string.IsNullOrWhiteSpace(messag))
            {
                var lobInfoFilter = JsonConvert.DeserializeObject<LobInfo>(messag);

                entity = this.GetLobInfo(lobInfoFilter);
            }
            else
            {
                entity = this.GetLobInfo();
            }

            mqttClient.Publish($"response_{topic}", JsonConvert.SerializeObject(entity));
        }

        public void SaveLobInfoMQtt(string topic, string messag)
        {
            LobInfo entity = new LobInfo();

            if (!string.IsNullOrWhiteSpace(messag))
            {
                entity = JsonConvert.DeserializeObject<LobInfo>(messag);

                this.SaveLobInfo(entity);
            }
            else
            {
                mqttClient.Publish($"response_{topic}", "Passe o objeto LobInfo");
            }

            mqttClient.Publish($"response_{topic}", JsonConvert.SerializeObject(entity));
        }


        #endregion

        #endregion

        #region BalanceInfo

        public BalanceInfo GetBalanceInfo()
        {
            return this.CoreSqlProvider.GetBalanceInfo();
        }

        public void SaveBalanceInfo(BalanceInfo entity)
        {
            if (entity.Id > 0)
                this.UpdateBalanceInfo(entity);
            else
                this.InsertBalanceInfo(entity);
        }

        public void UpdateBalanceInfo(BalanceInfo entity)
        {
            this.CoreSqlProvider.UpdateBalanceInfo(entity);
        }

        public void InsertBalanceInfo(BalanceInfo entity)
        {
            this.CoreSqlProvider.InsertBalanceInfo(entity);
        }

        #endregion

        #region Truck

        public List<Truck> GetTrucks()
        {
            var trucks = this.CoreSqlProvider.GetTrucks();

            if(trucks != null && trucks.Count > 0)
            {
                foreach(var item in trucks)
                {
                    item.TruckParam = this.GetTruckParam(item.Id);
                }
            }

            return trucks;
        }
        public List<Truck> GetTrucks(Truck entity)
        {
            var trucks = this.CoreSqlProvider.GetTrucks(entity);

            if (trucks != null && trucks.Count > 0)
            {
                foreach (var item in trucks)
                {
                    item.TruckParam = this.GetTruckParam(item.Id);
                }
            }

            return trucks;
        }

        public int SaveTruck(Truck entity)
        {
            if(entity.Id > 0)
            {
                this.CoreSqlProvider.UpdateTruck(entity);
            }
            else
            {
                var existSameTruckByName = this.GetTrucks(new Truck()
                {
                    Name = entity.Name
                }).FirstOrDefault();

                if(existSameTruckByName != null && existSameTruckByName.Id > 0)
                {
                    entity.Id = existSameTruckByName.Id;

                    this.CoreSqlProvider.UpdateTruck(entity);
                }
                else
                    this.CoreSqlProvider.InsertTruck(entity);
            }

            if (entity.TruckParam != null)
            {
                entity.TruckParam.TruckId = entity.Id;

                this.SaveTruckParam(entity.TruckParam);
            }

            return entity.Id;
        }
        public TruckParam GetTruckParam(int TruckId)
        {
            return this.CoreSqlProvider.GetTruckParam(TruckId);
        }

        public int SaveTruckParam(TruckParam entity)
        {
            if (entity.Id > 0)
            {
                this.CoreSqlProvider.UpdateTruckParam(entity);
            }
            else
            {
                if(entity.TruckId > 0)
                {
                    var existTruckParam = this.GetTruckParam(entity.TruckId.Value);

                    if (existTruckParam != null)
                    {
                        entity.Id = existTruckParam.Id;
                    }
                }

                this.CoreSqlProvider.InsertTruckParam(entity);
            }

            return entity.Id;
        }

        #endregion

        #region LobInfo

        public List<LobInfo> GetLobInfo()
        {
            var list = this.CoreSqlProvider.GetLobInfo();

            foreach(var item in list)
            {
                if(item.TruckId > 0)
                    item.Truck = this.GetTrucks(new Truck() { Id = item.TruckId.Value }).FirstOrDefault();
            }

            return list;
        }
        public List<LobInfo> GetLobInfo(LobInfo entity)
        {
            var list = this.CoreSqlProvider.GetLobInfo(entity);

            foreach (var item in list)
            {
                if (item.TruckId > 0)
                    item.Truck = this.GetTrucks(new Truck() { Id = item.TruckId.Value }).FirstOrDefault();
            }


            return this.CoreSqlProvider.GetLobInfo(entity);
        }

        public int SaveLobInfo(LobInfo entity)
        {
            if (entity.Id > 0)
            {
                this.CoreSqlProvider.UpdateLobInfo(entity);
            }
            else
            {
                this.CoreSqlProvider.InsertLobInfo(entity);
            }

            return entity.Id;
        }

        #endregion
    }
}
