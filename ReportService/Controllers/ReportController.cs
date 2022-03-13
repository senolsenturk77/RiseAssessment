using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReportService.Data.DTOs;
using ReportService.Data.Entities;
using ReportService.Data.Helper;
using ReportService.Data.Repositories;
using ReportService.MessageBus.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ReportService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {

        private readonly ILogger<ReportController> _logger;
        private readonly ReportRepository _reportRepository;
        private readonly IMessageBusClient _messageBusClient;

        public ReportController(ILogger<ReportController> logger, ReportRepository reportRepository, IMessageBusClient messageBusClient )
        {

            _logger = logger;
            _reportRepository = reportRepository;
            _messageBusClient = messageBusClient;
        }



        [HttpGet]
        public ActionResult<List<ReportDTO>> GetReports()
        {
            var reports = _reportRepository.GetAll().ToList();
            return Ok(reports.ToReportDTO());
        }


        [HttpGet("{uuid}")]
        public async Task<ActionResult> DownloadReport(Guid uuid)
        {

            var path = Path.Combine(Directory.GetCurrentDirectory(), "Reports");
            string sFileName = $@"Report-{uuid}.xlsx";
            FileInfo file = new FileInfo(Path.Combine(path, sFileName));
      
            if (file.Exists)
            {
                file = new FileInfo(Path.Combine(path, sFileName));

                FileStream fileStream = new FileStream(file.FullName, FileMode.Open);

                fileStream.Position = 0;
                var contentType = "application/octet-stream";

                return File(fileStream, contentType, sFileName);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpPost]
        public async Task<ActionResult> CreateReport()
        {

            try
            {


                var newReport = new Report()
                { Uuid = Guid.NewGuid(), Date = DateTime.Now, ReportPath = "", ReportStatus = Data.Entities.Enums.ReportStatusEnum.Prepearing };


                await _reportRepository.Insert(newReport);
                _messageBusClient.PublishNewReportRequest(newReport.Uuid);


                return Ok(newReport.Uuid.ToString());


            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }





        }



    }
}
