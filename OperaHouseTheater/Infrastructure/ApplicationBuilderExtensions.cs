namespace OperaHouseTheater.Infrastructure
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using OperaHouseTheater.Data;
    using OperaHouseTheater.Data.Models;

    using static WebConstants;
    using static Areas.Admin.AdminConstants;


    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);


            SeedPerformanceTypes(services);
            SeedDepartments(services);
            SeedEmployeeCategories(services);


            SeedAdministrator(services);
            
            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<OperaHouseTheaterDbContext>();

            data.Database.Migrate();
        }

        private static void SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = AdministratorRoleName };

                    await roleManager.CreateAsync(role);

                    const string adminEmail = "admin@oht.com";
                    const string adminPassword = "admin123";

                    var user = new User
                    {
                        Email = adminEmail,
                        UserName = adminEmail,
                        FullName = "Administrator"
                    };

                    await userManager.CreateAsync(user, adminPassword);

                    await userManager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }

        private static void SeedEmployeeCategories(IServiceProvider services)
        {
            var data = services.GetRequiredService<OperaHouseTheaterDbContext>();
            
            if (data.EmployeeCategories.Any())
            {
                return;
            }

            data.EmployeeCategories.AddRange(new[]
            {
                new EmployeeCategory{CategoryName = "Примабалерина"},
                new EmployeeCategory{CategoryName = "Премиер-солист"},
                new EmployeeCategory{CategoryName = "Солист"},
                new EmployeeCategory{CategoryName = "Ансамбъл"},
                new EmployeeCategory{CategoryName = "Гост-Солист"},
                new EmployeeCategory{CategoryName = "Сопран"},
                new EmployeeCategory{CategoryName = "Мецосопран"},
                new EmployeeCategory{CategoryName = "Тенор"},
                new EmployeeCategory{CategoryName = "Баритон"},
                new EmployeeCategory{CategoryName = "Бас"},
                new EmployeeCategory{CategoryName = "Директор"},
                new EmployeeCategory{CategoryName = "Художествен ръководител"},
                new EmployeeCategory{CategoryName = "Заместник-директор"},
                new EmployeeCategory{CategoryName = "Главен-художник"},
                new EmployeeCategory{CategoryName = "Главен-диригент"}
            });

            data.SaveChanges();
        }

        public static void SeedDepartments(IServiceProvider services)
        {
            var data = services.GetRequiredService<OperaHouseTheaterDbContext>();
            
            if (data.Departments.Any())
            {
                return;
            }

            data.Departments.AddRange(new[]
            {
                new Department{ DepartmentName = "Балет"},
                new Department{ DepartmentName = "Опера"},
                new Department{ DepartmentName = "Мениджмънт"},
            });

            data.SaveChanges();
        }

        public static void SeedPerformanceTypes(IServiceProvider services)
        {
            var data = services.GetRequiredService<OperaHouseTheaterDbContext>();
            
            if (data.PerformanceTypes.Any())
            {
                return;
            }

            data.PerformanceTypes.AddRange(new[]
            {
                new PerformanceType{Type = "Опера" },
                new PerformanceType{Type = "Балет" },
            });

            data.SaveChanges();
        }
    }
}
