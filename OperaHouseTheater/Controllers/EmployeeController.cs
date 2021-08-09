namespace OperaHouseTheater.Controllers.Employee
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Infrastructure;
    using OperaHouseTheater.Models.Employee;
    using OperaHouseTheater.Services.Admins;
    using OperaHouseTheater.Services.Employees;
    using System.Linq;

    public class EmployeeController : Controller
    {
        //private readonly OperaHouseTheaterDbContext data;
        private readonly IEmployeeService employees;
        private readonly IAdminService admins;

        public EmployeeController(/*OperaHouseTheaterDbContext data,*/ 
            IEmployeeService employees,
            IAdminService admins)
        {
            //this.data = data;
            this.employees = employees;
            this.admins = admins;
        }

        [Authorize]
        //only Admin
        public IActionResult Add() 
        {
            if (!this.admins.UserIsAdmin(this.User.GetId()))
            {
                //TODO Error message

                return BadRequest();
            }

            return View(new AddEmployeeFormModel
            {
                EmployeeCategories = this.employees.GetEmployeeCategories(),
                EmployeeDepartments = this.employees.GetEmployeeDepartments(),
            });
        }

        [HttpPost]
        [Authorize]
        //only Admin
        public IActionResult Add(AddEmployeeFormModel employeeInput) 
        {
            if (!this.admins.UserIsAdmin(this.User.GetId()))
            {
                //TODO Error message
                return BadRequest();
            }

            if (employeeInput.DepartmentId == 0)
            {
                this.ModelState.AddModelError(nameof(employeeInput.DepartmentId), "You must chose department.");
            }

            if (employeeInput.CategoryId == 0)
            {
                this.ModelState.AddModelError(nameof(employeeInput.CategoryId), "You must chose category.");
            }

            if (!this.employees.GetEmployeeDepartments().Any(d => d.Id == employeeInput.DepartmentId))
            {
                this.ModelState.AddModelError(nameof(employeeInput.DepartmentId), "This department does not exist.");
            }

            if (!this.employees.GetEmployeeCategories().Any(ec => ec.Id == employeeInput.CategoryId))
            {
                this.ModelState.AddModelError(nameof(employeeInput.CategoryId), "This category does not exist.");
            }
            
            if (!ModelState.IsValid)
            {
                employeeInput.EmployeeCategories = this.employees.GetEmployeeCategories();
                employeeInput.EmployeeDepartments =this.employees.GetEmployeeDepartments();

                return View(employeeInput);
            }

            employees.Add(
                employeeInput.FirstName,
                employeeInput.LastName,
                employeeInput.ImageUrl,
                employeeInput.Biography,
                employeeInput.DepartmentId,
                employeeInput.CategoryId);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult BalletEmployees()
        {
            var balletEmployees = this.employees.BalletEmployees();

            return View(balletEmployees);
        }

        public IActionResult OperaEmployees()
        {
            var operaEmployees = this.employees.OperaEmployees();

            return View(operaEmployees);
        }

        public IActionResult ManagementEmployees()
        {
            var managementEmployees = this.employees.МanagementEmployees();

            return View(managementEmployees);
        }

        public IActionResult Details(int id) 
        {
            var employeeData = this.employees.Details(id);

            if (employeeData == null)
            {
                // TODO Error Message

                return BadRequest();
            }

            return View(employeeData);
        }

        [Authorize]
        //only Admin
        public IActionResult Delete(int id)
        {
            if (!this.admins.UserIsAdmin(this.User.GetId()))
            {
                //TODO Error message

                return BadRequest();
            }

            var employeeExist = this.employees.Delete(id);

            if (employeeExist == false)
            {
                //TODO Error message

                return BadRequest();
            }

            return RedirectToAction("Index","Home");
        }

        //private bool ThisUserIsAdmin()
        //    => this.employees.UserIsAdmin(this.User.GetId());
    }
}
