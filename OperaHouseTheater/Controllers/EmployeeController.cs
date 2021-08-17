namespace OperaHouseTheater.Controllers.Employee
{
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Services.Employees;

    using static WebConstants;

    public class EmployeeController : Controller
    {
        private readonly IEmployeeService employees;

        public EmployeeController(IEmployeeService employees)
            => this.employees = employees;

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
                TempData["ErrorMessage"] = "Тhis person does not exist.";

                return RedirectToAction("Error", "Home");
            }

            return View(employeeData);
        }
    }
}
