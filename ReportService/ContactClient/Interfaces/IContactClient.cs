using ReportService.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReportService.ContactClient.Interfaces
{
    public interface IContactClient
    {
        Task<IEnumerable<ContactInfoDTO>> GetAllContactInfos();
    }
}
