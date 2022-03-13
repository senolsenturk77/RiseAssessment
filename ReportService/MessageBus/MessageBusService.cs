using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using OfficeOpenXml;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ReportService.ContactClient.Interfaces;
using ReportService.Data.DTOs;
using ReportService.Data.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ReportService.MessageBus
{
    public class MessageBusService : BackgroundService
    {

        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;

        private IConnection _connection;
        private IModel _channel;
        private string _queueName;


        public MessageBusService(IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _configuration = configuration;
            _scopeFactory = scopeFactory;

            InitializeMessageService();
        }





        private void InitializeMessageService()
        {

            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQServer"],
                Port = int.Parse(_configuration["RabbitMQPort"]),
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(type: ExchangeType.Fanout, exchange: "reportexchange");
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _queueName, exchange: "reportexchange", routingKey: "");

        }



        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ModuleHandle, ea) =>
            {
                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                var message = JsonConvert.DeserializeObject<ReportMessage>(notificationMessage);

                CreateRepor(message);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }


        public override void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
            base.Dispose();
        }





        private void CreateRepor(ReportMessage reportMessage)
        {

            List<ReportResultDTO> reportResultList = new List<ReportResultDTO>();


            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ReportRepository>();

                var contactDataClient = scope.ServiceProvider.GetRequiredService<IContactClient>();


                try
                {
                    var report = repo.GetById(reportMessage.MessageID).Result;

                    var contactInformationList = contactDataClient.GetAllContactInfos().Result;


                    var locations = contactInformationList.Where(w => w.InfoType == Data.Entities.Enums.ContactInfoType.Location)
                        .GroupBy(x => new { x.InfoTypeName, x.Info })
                        .Select(x => x.Key.Info);


                    foreach (var location in locations)
                    {
                        ReportResultDTO reportResultDto = new ReportResultDTO();

                        reportResultDto.Location = location;

                        reportResultDto.CountOfContact = contactInformationList.Where(x => x.InfoType == Data.Entities.Enums.ContactInfoType.Location
                         && x.Info == location).GroupBy(x => x.ContactUuid).Count();

                        var contatsOnLocation = contactInformationList.Where(x => x.InfoType == Data.Entities.Enums.ContactInfoType.Location
                       && x.Info == location).GroupBy(x => x.ContactUuid).Select(x => x.Key).ToList();

                        reportResultDto.CountOfPhoneNumber = contactInformationList.Where(x => x.InfoType == Data.Entities.Enums.ContactInfoType.PhoneNumber && contatsOnLocation.Contains(x.ContactUuid)).Count();
                        reportResultList.Add(reportResultDto);
                    }

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "Reports");
                    string sFileName = $@"Report-{reportMessage.MessageID}.xlsx";

                    FileInfo file = new FileInfo(Path.Combine(path, sFileName));
                    if (file.Exists)
                    {
                        file.Delete();
                        file = new FileInfo(Path.Combine(path, sFileName));
                    }
                    using (ExcelPackage package = new ExcelPackage(file))
                    {

                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("LocationReport");
                        worksheet.Cells[1, 1].Value = "Location";
                        worksheet.Cells[1, 2].Value = "Count of Contacts";
                        worksheet.Cells[1, 3].Value = "Count of Phone Numbers";

                        for (int i = 0; i < reportResultList.Count; i++)
                        {
                            worksheet.Cells[$"A{i + 2}"].Value = reportResultList[i].Location;
                            worksheet.Cells[$"B{i + 2}"].Value = reportResultList[i].CountOfContact;
                            worksheet.Cells[$"C{i + 2}"].Value = reportResultList[i].CountOfPhoneNumber;
                        }
                        package.Save();
                    }
                    report.ReportStatus = Data.Entities.Enums.ReportStatusEnum.Ready;

                    repo.Update(report).Wait();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }


        }
    }
}
