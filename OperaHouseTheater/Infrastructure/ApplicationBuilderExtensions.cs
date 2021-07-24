namespace OperaHouseTheater.Infrastructure
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using OperaHouseTheater.Data;
    using OperaHouseTheater.Data.Models;
    using System;
    using System.Linq;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app) 
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

           var data = scopedServices.ServiceProvider.GetService<OperaHouseTheaterDbContext>();

            data.Database.Migrate();

            SeedPerformanceTypes(data);
            SeedDepartments(data);
            SeedEmployeeCategories(data);
            return app;
        }

        private static void SeedEmployeeCategories(OperaHouseTheaterDbContext data)
        {
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

        public static void SeedDepartments(OperaHouseTheaterDbContext data) 
        {
            if (data.Departments.Any())
            {
                return;
            }

            data.Departments.AddRange(new[]
            {
                new Department{ DepartmentName = "Балет"},
                new Department{ DepartmentName = "Опера"},
                new Department{ DepartmentName = "Мениджмънд"},
            });

            data.SaveChanges();
        }

        public static void SeedPerformanceTypes(OperaHouseTheaterDbContext data) 
        {
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
