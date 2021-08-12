namespace OperaHouseTheater.Controllers.Employee
{
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Models.Employee;
    using OperaHouseTheater.Services.Employees;
    using OperaHouseTheater.Infrastructure;

    using static WebConstants;

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
                //TODO Error message

                TempData["ErrorMessage"] = "Some text";

                return RedirectToAction("Error", "Home");
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
                //TODO Error message

                return RedirectToAction("Error", "Home");
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

                return RedirectToAction("Error", "Home");
            }

            return View(employeeData);
        }

        [Authorize]
        //only Admin
        public IActionResult Delete(int id)
        {
            if (User.IsAdmin())
            {
                //TODO Error message

                return RedirectToAction("Error", "Home");
            }

            var employeeExist = this.employees.Delete(id);

            if (employeeExist == false)
            {
                //TODO Error message

                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
