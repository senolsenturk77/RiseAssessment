using ContactService.Data.Entities;
using ContactService.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactService.Data.Repositories
{
    public class ContactInfoRepository : IContactInfoRepository
    {
        ContactDbContext _dbContext;

        public ContactInfoRepository(ContactDbContext dbContext)
        {

            _dbContext = dbContext;
        }

        /// <summary>
        /// ContactInfo kaydını silmek için kullanılır
        /// </summary>
        /// <param name="Uuid">Guid tipinde Contact'a it id bilgisi</param>
        public async Task Delete(Guid Uuid)
        {

            var _record = await GetById(Uuid);

            if (_record == null) return;

            _dbContext.ContactInfos.Remove(_record);
            await _dbContext.SaveChangesAsync();

        }


        /// <summary>
        /// Tüm ContactInfo kayıtları için sorgulanabilir yapı döner
        /// </summary>
        /// <returns>IQueryable<ContactInfo></returns>
        public IQueryable<ContactInfo> GetAll()
        {
            return _dbContext.ContactInfos;
        }

        /// <summary>
        ///Contact'a ait tüm ContactInfo kayıtlarını döner
        /// </summary>
        /// <param name="Uuid">Guid tipinde Contact'a ait id bilgisi</param>
        /// <returns>IEnumerable<ContactInfo></returns>
        public IEnumerable<ContactInfo> GetContactInfosForContact(Guid Uuid)
        {
            return GetAll().Where(x => x.ContactUuid == Uuid).ToList();
        }

        /// <summary>
        /// Verilen Uuid değerine sahip tek bir ContactInfo kaydı döner
        /// </summary>
        /// <param name="Uuid">Guid tipinde ContactInfo'ya ait id bilgisi</param>
        /// <returns>ContactInfo</returns>
        public async Task<ContactInfo> GetById(Guid Uuid)
        {
            return await _dbContext.ContactInfos.AsNoTracking().FirstOrDefaultAsync(f => f.Uuid == Uuid);
        }

        /// <summary>
        /// Yeni bir ContactInfo kaydı ekler
        /// </summary>
        /// <param name="entity">ContactInfo</param>
        public async Task Insert(ContactInfo entity)
        {
            entity.Uuid = Guid.NewGuid();

            await _dbContext.ContactInfos.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Verilen Uuid değerine sahip ContactInfo kaydının varlığı kontrol edilir
        /// </summary>
        /// <param name="Uuid">Guid tipinde ContactInfo'ya ait id bilgisi</param>
        /// <returns>bool</returns>
        public async Task<bool> Exists(Guid Uuid)
        {
            return await _dbContext.ContactInfos.AnyAsync(a => a.Uuid == Uuid);
        }

        /// <summary>
        /// Parametre olarak verilen ContactInfo kaydını günceller
        /// </summary>
        /// <param name="entity">ContactInfo tipinde kayıt verisi</param>
        public async Task Update(ContactInfo entity)
        {
            _dbContext.ContactInfos.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}