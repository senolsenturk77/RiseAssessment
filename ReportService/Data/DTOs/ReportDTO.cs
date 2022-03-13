using ReportService.Data.Entities;
using ReportService.Data.Entities.Enums;
using System;

namespace ReportService.Data.DTOs
{
    public class ReportDTO: BaseEntity
    {

        public DateTime Date { get; set; }

        public string ReportStatus { get; set; }


    }
}
