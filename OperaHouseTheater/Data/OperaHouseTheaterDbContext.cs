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
    }
}
