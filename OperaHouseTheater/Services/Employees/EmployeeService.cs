namespace OperaHouseTheater.Services.Employees
{
    using Microsoft.AspNetCore.Authorization;
    using OperaHouseTheater.Data;
    using OperaHouseTheater.Data.Models;
    using OperaHouseTheater.Models.Employee;
    using System.Collections.Generic;
    using System.Linq;
    public class EmployeeService : IEmployeeService
    {
        private readonly OperaHouseTheaterDbContext data;

        public EmployeeService(OperaHouseTheaterDbContext data) 
            => this.data = data;

        public void Add(
            string firstName, 
            string lastName, string imageUrl, 
            string biography, int departmentId, 
            int categoryId)
        {

            var employeeData = new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                ImageUrl = imageUrl,
                Biography = biography,
                DepartmentId = departmentId,
                CategoryId = categoryId
            };

            this.data.Employees.Add(employeeData);
            this.data.SaveChanges();
        }

        public EmployeeQueryServiceModel BalletEmployees()
        {
            var balletEmployees = GetEmployees(this.data.Employees
                .Where(e => e.Department.DepartmentName == "Балет"));

            return new EmployeeQueryServiceModel 
            {
                Employees = balletEmployees
            };
        }

        public EmployeeQueryServiceModel OperaEmployees()
        {
            var operaEmployees = GetEmployees(this.data.Employees
                .Where(e => e.Department.DepartmentName == "Опера"));

            return new EmployeeQueryServiceModel
            {
                Employees = operaEmployees
            };
        }

        public EmployeeQueryServiceModel МanagementEmployees()
        {
            var managementEmployees = GetEmployees(this.data.Employees
                .Where(e => e.Department.DepartmentName == "Мениджмънт"));

            return new EmployeeQueryServiceModel
            {
                Employees = managementEmployees
            };
        }

        public EmployeeDetailsServiceModel Details(int id)
        {
            var employee = this.data
                .Employees
                .FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return null;
            }

            var employeeData = new EmployeeDetailsServiceModel
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Biography = employee.Biography,
                ImageUrl = employee.ImageUrl
            };

            return employeeData;
        }

        
        public bool Delete(int id)
        {
            var employee = this.data.Employees
                .FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return false;
            }
            else
            {
                this.data.Employees.Remove(employee);
                this.data.SaveChanges();
            }

            return true;
        }

        public static IEnumerable<EmployeeServiceModel> GetEmployees(IQueryable<Employee> employeesQuery)
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

        public IEnumerable<EmployeeDepartmentServiceModel> GetEmployeeDepartments()
             => this.data
                 .Departments
                 .Select(d => new EmployeeDepartmentServiceModel
                 {
                     Id = d.Id,
                     DepartmentName = d.DepartmentName
                 })
                 .ToList();

        public IEnumerable<EmployeeCategoryServiceModel> GetEmployeeCategories()
            => this.data
                .EmployeeCategories
                .Select(p => new EmployeeCategoryServiceModel
                {
                    Id = p.Id,
                    CategoryName = p.CategoryName
                })
                .ToList();

        public bool UserIsAdmin(string userId)
            => this.data
                .Admins
                .Any(x => x.UserId == userId);

        public bool EmployeeIsInEventRole(int id)
            => this.data
            .EventRoles
            .Any(er => er.EmployeeId == id);
    }
}
