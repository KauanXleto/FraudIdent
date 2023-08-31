using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FraudIdent.Backbone.BusinessRules
{
    public class MqttClientRequest
    {
        public IMqttClient _client;
        private readonly MqttClientOptions options;

        private bool _isAwait = false;

        public MqttClientRequest(string brokerUrl, int brokerPort, string clientId)
        {
            MqttClientOptions _options = new MqttClientOptionsBuilder()
            .WithClientId(clientId)
            .WithTcpServer(brokerUrl, brokerPort)
            .WithKeepAlivePeriod(TimeSpan.FromMinutes(60))
            .WithCredentials("", "")
            .Build();

            options = _options;

            _client = new MqttFactory().CreateMqttClient();
        }

        public IMqttClient Connect(bool? _keepAlive = true)
        {
            if (!_client.IsConnected && !_isAwait)
            {
                try
                {
                    _isAwait = true;

                    var connection = _client.ConnectAsync(options, CancellationToken.None);
                    connection.Wait();

                    _isAwait = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro Connect");
                }

                if (_keepAlive == true)
                    keepAlive();
            }
            else if (!_client.IsConnected && _isAwait)
            {
                //Connect(_keepAlive);
            }

            return _client;
        }

        public void keepAlive()
        {
            if (!_client.IsConnected)
            {
                Connect();
            }

            Task.Delay(new TimeSpan(0, 0, 30)).ContinueWith(o => { keepAlive(); });
        }

        public void Subscribe(string topic)
        {
            _client.SubscribeAsync(topic, MqttQualityOfServiceLevel.ExactlyOnce);
        }

        public void Publish(string topic, string message)
        {
            message = (message != null ? message : "");

            if (_client.IsConnected)
            {
                _client.PublishAsync(new MqttApplicationMessage()
                {
                    Topic = topic,
                    Payload = Encoding.ASCII.GetBytes(message)
                });
            }
            else
            {
                Connect();
                Task.Delay(new TimeSpan(0, 0, 10)).ContinueWith(o => { Publish(topic, message); });
            }

        }
    }
}
