using ContactService.Data.Entities;
using System;
using System.Threading.Tasks;

namespace ContactService.Data.Repositories.Interfaces
{
    public interface IContactRepository:IRepository<Contact>
    {
        Task<Contact> GetWithInfoById(Guid Uuid);
    }
}
