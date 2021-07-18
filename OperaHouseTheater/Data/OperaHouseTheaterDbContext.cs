namespace OperaHouseTheater.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using OperaHouseTheater.Data.Models;

    public class OperaHouseTheaterDbContext : IdentityDbContext
    {

        public OperaHouseTheaterDbContext(DbContextOptions<OperaHouseTheaterDbContext> options)
            : base(options)
        {
        }

        public DbSet<News> News { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder
                .Entity<Employee>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Employees)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);



            base.OnModelCreating(builder);
        }
    }
}
