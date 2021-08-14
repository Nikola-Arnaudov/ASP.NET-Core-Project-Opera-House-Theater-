namespace OperaHouseTheater.Controllers.PerformanceRole
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using OperaHouseTheater.Models.Performance;
    using OperaHouseTheater.Services.Performances;
    using OperaHouseTheater.Services.Roles;
    using OperaHouseTheater.Infrastructure;

    public class RoleController : Controller
    {
        private readonly IRoleService roles;
        private readonly IPerformanceService performances;

        public RoleController(
            IRoleService roles,
            IPerformanceService performances)
        {
            this.roles = roles;
            this.performances = performances;
        }

        [Authorize]
        public IActionResult AddRole()
        {
            if (!User.IsAdmin())
            {
                TempData["ErrorMessage"] = "Аccess denied.";

                return RedirectToAction("Error", "Home");
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        //only Admin
        public IActionResult AddRole(AddRoleFormModel role)
        {
            if (!User.IsAdmin())
            {
                TempData["ErrorMessage"] = "Аccess denied.";

                return RedirectToAction("Error", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(role);
            }

            this.roles.Add(role.Name, role.PerformanceId);

            return Redirect($"/Performance/Details/{role.PerformanceId}");
        }

        [Authorize]
        //only Admin
        public IActionResult Delete(int id)
        {
            if (!User.IsAdmin())
            {
                TempData["ErrorMessage"] = "Аccess denied.";

                return RedirectToAction("Error", "Home");
            }

            var performanceId = this.roles.Delete(id);

            if (performanceId == 0)
            {
                TempData["ErrorMessage"] = "Role with this id it doesn't exist.";

                return RedirectToAction("Error", "Home");
            }

            return Redirect($"/Performance/Details/{performanceId}");
        }
    }
}
