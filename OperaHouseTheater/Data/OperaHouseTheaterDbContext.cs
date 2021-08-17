namespace OperaHouseTheater.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using OperaHouseTheater.Data.Models;
    using System;

    public class OperaHouseTheaterDbContext : IdentityDbContext<User>
    {
        public OperaHouseTheaterDbContext(DbContextOptions<OperaHouseTheaterDbContext> options)
            : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<EmployeeCategory> EmployeeCategories { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<EventRole> EventRoles { get; set; }

        public DbSet<Member> Members { get; set; }

        public DbSet<News> News { get; set; }

        public DbSet<Performance> Performances { get; set; }

        public DbSet<PerformanceType> PerformanceTypes { get; set; }

        public DbSet<Role> RolesPerformance { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Admin>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<Admin>(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Member>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<Member>(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Comment>()
                .HasOne(c => c.Member)
                .WithMany(c => c.Comments)
                .HasForeignKey(c => c.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Comment>()
                .HasOne(c => c.Performance)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PerformanceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Employee>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Employees)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(c => c.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Employee>()
                .HasOne(e => e.Category)
                .WithMany(d => d.Employees)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Event>()
                .HasOne(e => e.Performance)
                .WithMany(p => p.Events)
                .HasForeignKey(e => e.PerformanceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<EventRole>()
                .HasOne(e => e.Event)
                .WithMany(er => er.EventRoles)
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Ticket>()
                .HasOne(t => t.Member)
                .WithMany(m => m.Tickets)
                .HasForeignKey(t => t.MemberId)
                .OnDelete(DeleteBehavior.Restrict);


            builder
                .Entity<EventRole>()
                .HasOne(e => e.Employee)
                .WithMany(er => er.EventRoles)
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Role>()
                .HasOne(r => r.Performance)
                .WithMany(p => p.Roles)
                .HasForeignKey(r => r.PerformanceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Performance>()
                .HasOne(p => p.PerformanceType)
                .WithMany(pt => pt.Performances)
                .HasForeignKey(p => p.PerformanceTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Ticket>()
                .HasOne(e => e.Event)
                .WithMany(r => r.Tickets)
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
