using FileModificationSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace FileModificationSystem.Infrastructure
{
    class EmployeeDbContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=employee.db");

        public DbSet<Employee> Employee { get; set; }

        public DbSet<Address> Address { get; set; }

        public DbSet<Qualification> Qualification { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().Ignore(x => x.Qualification).Ignore(x => x.Address).HasKey(x => x.Id);
            modelBuilder.Entity<Address>().HasNoKey();
            modelBuilder.Entity<Qualification>().HasNoKey();
        }
    }

}
