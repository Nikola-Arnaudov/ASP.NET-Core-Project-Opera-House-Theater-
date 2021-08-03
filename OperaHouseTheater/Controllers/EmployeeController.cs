namespace OperaHouseTheater.Controllers.Employee
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Data;
    using OperaHouseTheater.Data.Models;
    using OperaHouseTheater.Infrastructure;
    using OperaHouseTheater.Models.Employee;
    using System.Collections.Generic;
    using System.Linq;

    public class EmployeeController : Controller
    {
        private readonly OperaHouseTheaterDbContext data;

        public EmployeeController(OperaHouseTheaterDbContext data)
            => this.data = data;

        [Authorize]
        public IActionResult Add() 
        {
            if (!UserIsAdmin())
            {

            }


            return View(new AddEmployeeFormModel
            {
                EmployeeCategories = this.GetEmployeeCategories(),
                EmployeeDepartments = this.GetEmployeeDepartments()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddEmployeeFormModel employeeInput) 
        {
            if (!this.data.Departments.Any(d=> d.Id == employeeInput.DepartmentId))
            {
                this.ModelState.AddModelError(nameof(employeeInput.DepartmentId), "This department does not exist.");
            }

            if (!this.data.EmployeeCategories.Any(ec => ec.Id == employeeInput.CategoryId))
            {
                this.ModelState.AddModelError(nameof(employeeInput.CategoryId), "This category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                employeeInput.EmployeeCategories = GetEmployeeCategories();
                employeeInput.EmployeeDepartments = GetEmployeeDepartments();

                return View(employeeInput);
            }

            var employeeData = new Employee
            { 
                FirstName = employeeInput.FirstName,
                LastName = employeeInput.LastName,
                ImageUrl = employeeInput.ImageUrl,
                Biography = employeeInput.Biography,
                DepartmentId = employeeInput.DepartmentId,
                CategoryId = employeeInput.CategoryId
            };

            this.data.Employees.Add(employeeData);
            this.data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult BalletEmployees()
        {
            var employees = this.data.Employees
                .Where(e => e.Department.DepartmentName == "Балет")
                .Select(e => new EmployeeListingViewModel
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Category = e.Category.CategoryName,
                    ImageUrl = e.ImageUrl,
                })
                .OrderBy(x=>x.FirstName)
                .ToList();

            return View(employees);
        }

        public IActionResult OperaEmployees()
        {
            var employees = this.data.Employees
                .Where(e => e.Department.DepartmentName == "Опера")
                .Select(e => new EmployeeListingViewModel
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Category = e.Category.CategoryName,
                    ImageUrl = e.ImageUrl,
                })
                .OrderBy(x => x.FirstName)
                .ToList();

            return View(employees);
        }

        public IActionResult ManagementEmployees()
        {
            var employees = this.data.Employees
                .Where(e => e.Department.DepartmentName == "Мениджмънд")
                .Select(e => new EmployeeListingViewModel
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Category = e.Category.CategoryName,
                    ImageUrl = e.ImageUrl,
                })
                .OrderBy(x => x.FirstName)
                .ToList();

            return View(employees);
        }

        public IActionResult Details(int id) 
        {
            var employee = this.data
                .Employees
                .FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return BadRequest();
            }

            var employeeData = new EmployeeDetailsViewModel
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Biography = employee.Biography,
                ImageUrl = employee.ImageUrl
            };

            return View(employeeData);
        }

        public IActionResult Delete(int id)
        {
            var employee = this.data.Employees
                .FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return BadRequest();
            }

            this.data.Employees.Remove(employee);
            this.data.SaveChanges();

            return RedirectToAction("Index","Home");
        }

        private bool UserIsAdmin()
            => this.data
                .Admins
                .Any(x => x.UserId == this.User.GetId());

        
        private IEnumerable<EmployeeDepartmentViewModel> GetEmployeeDepartments()
            => this.data
                .Departments
                .Select(d => new EmployeeDepartmentViewModel
                {
                    Id = d.Id,
                    DepartmentName = d.DepartmentName
                })
                .ToList();

        private IEnumerable<EmployeeCategoryViewModel> GetEmployeeCategories()
            => this.data
                .EmployeeCategories
                .Select(p => new EmployeeCategoryViewModel
                {
                    Id = p.Id,
                    CategoryName = p.CategoryName
                })
                .ToList();
    }
}
