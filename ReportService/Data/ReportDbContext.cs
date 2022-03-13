using Microsoft.EntityFrameworkCore;
using ReportService.Data.Entities;

namespace ReportService.Data
{
    public class ReportDbContext:DbContext
    {

        public DbSet<Report> Reports { get; set; }
        

        public ReportDbContext(DbContextOptions<ReportDbContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }




    }
}
