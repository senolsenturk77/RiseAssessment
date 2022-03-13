using ReportService.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportService.Data.Repositories.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {

        IQueryable<T> GetAll();
        Task<T> GetById(Guid id);
        Task<bool> Exists(Guid id);
        Task Insert(T entity);
        Task Update(T entity);
        Task Delete(Guid id);

    }
}
