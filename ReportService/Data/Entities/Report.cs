using ReportService.Data.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ReportService.Data.Entities
{
    public class Report : BaseEntity
    {
        [Required]
        public DateTime Date { get; set; }

        public ReportStatusEnum ReportStatus { get; set; } = ReportStatusEnum.Prepearing;


        [JsonIgnore]
        public string ReportPath { get; set; }



    }
}
