using ContactService.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactService.Data
{
    public class ContactDbContext:DbContext
    {





        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }



        public ContactDbContext(DbContextOptions<ContactDbContext> options):base(options)
        {


        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }




    }
}
