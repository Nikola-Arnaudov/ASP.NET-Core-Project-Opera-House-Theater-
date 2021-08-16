namespace OperaHouseTheater.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Services.Employees;
    using OperaHouseTheater.Infrastructure;
    using OperaHouseTheater.Models.Employee;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using static AdminConstants;

    [Area(AreaName)]
    [Authorize(Roles = AdministratorRoleName)]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService employees;

        public EmployeeController(IEmployeeService employees)
            => this.employees = employees;

        [Authorize]
        //only Admin
        public IActionResult Add()
        {
            if (!User.IsAdmin())
            {
                TempData["ErrorMessage"] = "Аccess denied.";

                return RedirectToAction("Error", "Home", new { area = "" });
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
            if (!User.IsAdmin())
            {
                TempData["ErrorMessage"] = "Аccess denied.";

                return RedirectToAction("Error", "Home", new { area = "" });
            }

            if (employeeInput.DepartmentId == 0)
            {
                this.ModelState.AddModelError(nameof(employeeInput.DepartmentId), "You must chose a department.");
            }

            if (employeeInput.CategoryId == 0)
            {
                this.ModelState.AddModelError(nameof(employeeInput.CategoryId), "You must chose a category.");
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
                employeeInput.EmployeeDepartments = this.employees.GetEmployeeDepartments();

                return View(employeeInput);
            }

            employees.Add(
                employeeInput.FirstName,
                employeeInput.LastName,
                employeeInput.ImageUrl,
                employeeInput.Biography,
                employeeInput.DepartmentId,
                employeeInput.CategoryId);

            return RedirectToAction("Index", "Home", new { area = "" });
        }

        [Authorize]
        //only Admin
        public IActionResult Delete(int id)
        {
            if (!User.IsAdmin())
            {
                TempData["ErrorMessage"] = "Аccess denied.";

                return RedirectToAction("Error", "Home", new { area = ""});
            }

            var employeeHaveRoleEvents = this.employees.EmployeeIsInEventRole(id);

            if (employeeHaveRoleEvents)
            {
                TempData["ErrorMessage"] = "Can't delete this employee, because he have a role in comming event.";

                return RedirectToAction("Error", "Home", new { area = "" });
            }

            var employeeExist = this.employees.Delete(id);

            if (employeeExist == false)
            {
                TempData["ErrorMessage"] = "Employee with that id does not exist.";

                return RedirectToAction("Error", "Home", new { area = "" });
            }

            return RedirectToAction("Index", "Home", new { area = "" });
        }


    }
}
