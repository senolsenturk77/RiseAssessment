using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using ReportService.MessageBus.Interfaces;
using System;
using System.Text;
using System.Text.Json;

namespace ReportService.MessageBus
{
    public class RabbitMQClient: IMessageBusClient, IDisposable
    {

        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;


        public RabbitMQClient(IConfiguration configuration)
        {
            _configuration = configuration;


            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQServer"],
                Port = int.Parse(_configuration["RabbitMQPort"]),
                UserName="guest",
                Password="guest"
            };



            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: "reportexchange", type: ExchangeType.Fanout);
                

            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't connect to Message Bus: {ex.Message}");
            }

        }


        public void PublishNewReportRequest(Guid messageGuid)
        {

            var newReportMessage = new ReportMessage() { MessageID = messageGuid};
            var message = JsonConvert.SerializeObject(newReportMessage);
            
            if (_connection.IsOpen)
            {
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("No connection is open");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "reportexchange", routingKey: "", basicProperties: null, body: body);
            
            
            Console.WriteLine($"{message} is sended.");
        }

        public void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

       

    }
}
