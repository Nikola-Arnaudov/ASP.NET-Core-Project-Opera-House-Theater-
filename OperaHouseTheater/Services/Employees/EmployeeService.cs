namespace OperaHouseTheater.Services.Employees
{
    using OperaHouseTheater.Data;
    using OperaHouseTheater.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class EmployeeService : IEmployeeService
    {
        private readonly OperaHouseTheaterDbContext data;

        public EmployeeService(OperaHouseTheaterDbContext data) 
            => this.data = data;

        public EmployeeQueryServiceModel BalletEmployees()
        {
            //var balletEmployees = this.data.Employees
            //    .Where(e => e.Department.DepartmentName == "Балет")
            //    .Select(e => new EmployeeServiceModel
            //    {
            //        Id = e.Id,
            //        FirstName = e.FirstName,
            //        LastName = e.LastName,
            //        Category = e.Category.CategoryName,
            //        ImageUrl = e.ImageUrl,
            //    })
            //    .OrderBy(x => x.FirstName)
            //    .ToList();

            var balletEmployees = GetEmployees(this.data.Employees
                .Where(e => e.Department.DepartmentName == "Балет"));

            return new EmployeeQueryServiceModel 
            {
                Employees = balletEmployees
            };
        }

        public EmployeeQueryServiceModel OperaEmployees()
        {
            //var operaEmployees = this.data.Employees
            //    .Where(e => e.Department.DepartmentName == "Опера")
            //    .Select(e => new EmployeeServiceModel
            //    {
            //        Id = e.Id,
            //        FirstName = e.FirstName,
            //        LastName = e.LastName,
            //        Category = e.Category.CategoryName,
            //        ImageUrl = e.ImageUrl,
            //    })
            //    .OrderBy(x => x.FirstName)
            //    .ToList();

            var operaEmployees = GetEmployees(this.data.Employees
                .Where(e => e.Department.DepartmentName == "Опера"));

            return new EmployeeQueryServiceModel
            {
                Employees = operaEmployees
            };
        }

        public EmployeeQueryServiceModel МanagementEmployees()
        {
            //var managementEmployees = this.data.Employees
            //    .Where(e => e.Department.DepartmentName == "Мениджмънд")
            //    .Select(e => new EmployeeServiceModel
            //    {
            //        Id = e.Id,
            //        FirstName = e.FirstName,
            //        LastName = e.LastName,
            //        Category = e.Category.CategoryName,
            //        ImageUrl = e.ImageUrl,
            //    })
            //    .OrderBy(x => x.FirstName)
            //    .ToList();

            var managementEmployees = GetEmployees(this.data.Employees
                .Where(e => e.Department.DepartmentName == "Мениджмънд"));

            return new EmployeeQueryServiceModel
            {
                Employees = managementEmployees
            };
        }

        private static IEnumerable<EmployeeServiceModel> GetEmployees(IQueryable<Employee> employeesQuery)
            => employeesQuery
            .Select(e => new EmployeeServiceModel
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Category = e.Category.CategoryName,
                ImageUrl = e.ImageUrl,
            })
                .OrderBy(x => x.FirstName)
                .ToList();


    }
}
