using Microsoft.EntityFrameworkCore;
using ReportService.Data.Entities;
using ReportService.Data.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReportService.Data.Repositories
{

    public class ReportRepository : IRepository<Report>
    {

        ReportDbContext _dbContext;

        public ReportRepository(ReportDbContext dbContext)
        {

            _dbContext = dbContext;
        }


        /// <summary>
        /// Report kaydını silmek için kullanılır
        /// </summary>
        /// <param name="Uuid">Guid tipinde id bilgisi</param>
        public async Task Delete(Guid Uuid)
        {

            var _record = await GetById(Uuid);

            if (_record == null) return;

            _dbContext.Reports.Remove(_record);
            await _dbContext.SaveChangesAsync();

        }


        /// <summary>
        /// Tüm Report kayıtları için sorgulanabilir yapı döner
        /// </summary>
        /// <returns>IQueryable<Contact></returns>
        public IQueryable<Report> GetAll()
        {
            return _dbContext.Reports;
        }


        /// <summary>
        ///Bir Report kaydı döner
        /// </summary>
        /// <param name="Uuid">Guid tipinde Report'a ait id bilgisi</param>
        /// <returns>Report</returns>
        public async Task<Report> GetById(Guid Uuid)
        {
            return await _dbContext.Reports.AsNoTracking().FirstOrDefaultAsync(f => f.Uuid == Uuid);
        }

        /// <summary>
        /// Yeni bir Report kaydı ekler
        /// </summary>
        /// <param name="entity">Report</param>
        public async Task Insert(Report entity)
        {

            entity.Uuid = Guid.NewGuid();

            await _dbContext.Reports.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }


        /// <summary>
        /// Verilen Uuid değerine sahip Report kaydının varlığı kontrol edilir
        /// </summary>
        /// <param name="Uuid">Guid tipinde Report'a ait Uuid bilgisi</param>
        /// <returns>bool</returns>
        public async Task<bool> Exists(Guid Uuid)
        {
            return await _dbContext.Reports.AnyAsync(a => a.Uuid == Uuid);
        }


        /// <summary>
        /// Parametre olarak verilen Report kaydını günceller
        /// </summary>
        /// <param name="entity">Report tipinde kayıt verisi</param>
        public async Task Update(Report entity)
        {
            _dbContext.Reports.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}