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

        public async Task Delete(Guid Uuid)
        {

            var _record = await GetById(Uuid);

            if (_record == null) return;

            _dbContext.Contacts.Remove(_record);
            await _dbContext.SaveChangesAsync();

        }

        public IQueryable<Contact> GetAll()
        {
            return _dbContext.Contacts;
        }

        public async Task<Contact> GetById(Guid Uuid)
        {
            return await _dbContext.Contacts.AsNoTracking().FirstOrDefaultAsync(f => f.Uuid == Uuid);
        }



        public async Task<Contact> GetWithInfoById(Guid Uuid)
        {
            return await _dbContext.Contacts.Include(p => p.ContactInformations).AsNoTracking().FirstOrDefaultAsync(f => f.Uuid == Uuid);
        }


        public async Task Insert(Contact entity)
        {

            entity.Uuid = Guid.NewGuid();

            await _dbContext.Contacts.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> Exists(Guid Uuid)
        {
            return await _dbContext.Contacts.AnyAsync(a => a.Uuid == Uuid);
        }

        public async Task Update(Contact entity)
        {
            _dbContext.Contacts.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
