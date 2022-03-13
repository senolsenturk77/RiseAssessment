
using ReportService.Data.DTOs;
using ReportService.Data.Entities;
using System.Collections.Generic;

namespace ReportService.Data.Helper
{
    public static class HelperDTO
    {
            


        public static ReportDTO ToReportDTO(this Report report)
        {

            var resReportDTO = new ReportDTO();

            resReportDTO.Uuid = report.Uuid;
            resReportDTO.Date = report.Date;
            resReportDTO.ReportStatus = report.ReportStatus.ToString();
            
            return resReportDTO;

        }




        public static List<ReportDTO> ToReportDTO(this IEnumerable<Report> reports)
        {

            var retList = new List<ReportDTO>();

            foreach (var report in reports)
            {
                retList.Add(report.ToReportDTO());
            }

            return retList;

        }


    }
}
