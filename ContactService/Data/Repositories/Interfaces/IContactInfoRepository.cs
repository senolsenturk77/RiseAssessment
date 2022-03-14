using ContactService.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactService.Data.Repositories.Interfaces
{
    public interface IContactInfoRepository:IRepository<ContactInfo>
    {
        IEnumerable<ContactInfo> GetContactInfosForContact(Guid Uuid);
    }
}
