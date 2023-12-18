using EvoSystems.Models;
using Microsoft.EntityFrameworkCore;

namespace EvoSystems.DataContext
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<Department> Department { get; set; }
        public DbSet<Employee> Employee { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasIndex(e => e.RG).IsUnique();
            modelBuilder.Entity<Department>().HasIndex(e => e.Name).IsUnique();
            modelBuilder.Entity<Department>().HasIndex(e => e.Abbreviation).IsUnique();
            modelBuilder.Entity<Department>().Property(e => e.Abbreviation).IsRequired().HasMaxLength(3);
            modelBuilder.Entity<Employee>().Property(e => e.RG).IsRequired().HasMaxLength(9);
        }
    }
}
