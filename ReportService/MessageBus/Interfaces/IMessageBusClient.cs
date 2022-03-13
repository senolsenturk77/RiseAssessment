using System;

namespace ReportService.MessageBus.Interfaces
{
    public interface IMessageBusClient
    {
        void PublishNewReportRequest(Guid messageGuid);
    }
}
