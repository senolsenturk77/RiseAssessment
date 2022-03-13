using ContactService.Data.Entities;
using ContactService.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ContactService.Data.Repositories
{
    public class ContactRepository : IRepository<Contact>
    {

        ContactDbContext _dbContext;

        public ContactRepository(ContactDbContext dbContext)
        {

            _dbContext = dbContext;
        }


        /// <summary>
        /// Contact kaydını silmek için kullanılır
        /// </summary>
        /// <param name="Uuid">Guid tipinde id bilgisi</param>
        public async Task Delete(Guid Uuid)
        {

            var _record = await GetById(Uuid);

            if (_record == null) return;

            _dbContext.Contacts.Remove(_record);
            await _dbContext.SaveChangesAsync();

        }


        /// <summary>
        /// Tüm Contact kayıtları için sorgulanabilir yapı döner
        /// </summary>
        /// <returns>IQueryable<Contact></returns>
        public IQueryable<Contact> GetAll()
        {
            return _dbContext.Contacts;
        }


        /// <summary>
        ///Bir Contact kaydı döner
        /// </summary>
        /// <param name="Uuid">Guid tipinde Contact'a ait id bilgisi</param>
        /// <returns>Contact</returns>
        public async Task<Contact> GetById(Guid Uuid)
        {
            return await _dbContext.Contacts.AsNoTracking().FirstOrDefaultAsync(f => f.Uuid == Uuid);
        }


        /// <summary>
        /// Uuid değeri verilen Contact kayıdı ilişkili ContactInfo verileri ile beraber döndürür.
        /// </summary>
        /// <param name="Uuid">Contact'a ait Uuid değeri</param>
        /// <returns>Contact</returns>
        public async Task<Contact> GetWithInfoById(Guid Uuid)
        {
            return await _dbContext.Contacts.Include(p => p.ContactInformations).AsNoTracking().FirstOrDefaultAsync(f => f.Uuid == Uuid);
        }


        /// <summary>
        /// Yeni bir Contact kaydı ekler
        /// </summary>
        /// <param name="entity">Contact</param>
        public async Task Insert(Contact entity)
        {

            entity.Uuid = Guid.NewGuid();

            await _dbContext.Contacts.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }


        /// <summary>
        /// Verilen Uuid değerine sahip Contact kaydının varlığı kontrol edilir
        /// </summary>
        /// <param name="Uuid">Guid tipinde Contact'a ait Uuid bilgisi</param>
        /// <returns>bool</returns>
        public async Task<bool> Exists(Guid Uuid)
        {
            return await _dbContext.Contacts.AnyAsync(a => a.Uuid == Uuid);
        }


        /// <summary>
        /// Parametre olarak verilen Contact kaydını günceller
        /// </summary>
        /// <param name="entity">Contact tipinde kayıt verisi</param>
        public async Task Update(Contact entity)
        {
            _dbContext.Contacts.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
