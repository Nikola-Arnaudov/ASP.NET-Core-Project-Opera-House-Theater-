namespace OperaHouseTheater.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Services.Roles;
    using OperaHouseTheater.Infrastructure;
    using OperaHouseTheater.Models.Performance;

    using static AdminConstants;

    [Area(AreaName)]
    [Authorize(Roles = AdministratorRoleName)]
    public class RoleController : Controller
    {
        private readonly IRoleService roles;

        public RoleController(IRoleService roles)
            => this.roles = roles;

        [Authorize]
        public IActionResult AddRole()
        {
            if (!User.IsAdmin())
            {
                TempData["ErrorMessage"] = "Аccess denied.";

                return RedirectToAction("Error", "Home", new { area = "" });
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

                return RedirectToAction("Error", "Home", new { area = "" });
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

                return RedirectToAction("Error", "Home", new { area = "" });
            }

            var performanceId = this.roles.Delete(id);

            if (performanceId == 0)
            {
                TempData["ErrorMessage"] = "Role with this id it doesn't exist.";

                return RedirectToAction("Error", "Home", new { area = "" });
            }

            return Redirect($"/Performance/Details/{performanceId}");
        }


    }
}
