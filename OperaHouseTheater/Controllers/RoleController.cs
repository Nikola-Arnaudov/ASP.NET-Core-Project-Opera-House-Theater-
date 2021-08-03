namespace OperaHouseTheater.Controllers.PerformanceRole
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Data;
    using OperaHouseTheater.Data.Models;
    using OperaHouseTheater.Models.Performance;

    public class RoleController : Controller
    {
        private readonly OperaHouseTheaterDbContext data;

        public RoleController(OperaHouseTheaterDbContext data)
            => this.data = data;

        public IActionResult AddRole() => View(new AddRoleFormModel
        {
            PerformanceTitles = this.GetPerformanceTitles()
        });

        [HttpPost]
        public IActionResult AddRole(AddRoleFormModel role)
        {
            if (!this.data.Performances.Any(p => p.Id == role.PerformanceId))
            {
                this.ModelState.AddModelError(nameof(role.PerformanceTitles), "This performance does not exist.");
            }

            if (!ModelState.IsValid)
            {
                role.PerformanceTitles = this.GetPerformanceTitles();

                return View(role);
            }

            var roleData = new Role
            {
                RoleName = role.Name,
                PerformanceId = role.PerformanceId
            };

            this.data.RolesPerformance.Add(roleData);
            this.data.SaveChanges();

            return Redirect($"/Performance/Details/{role.PerformanceId}");
        }

        public IActionResult Delete(int id) 
        {
            var role = data
                .RolesPerformance
                .Where(r => r.Id == id)  
                .FirstOrDefault();

            if (role == null)
            {
                return BadRequest();
            }

            var performanceId = role.PerformanceId;

            this.data.RolesPerformance.Remove(role);
            this.data.SaveChanges();

            return Redirect($"/Performance/Details/{performanceId}");
        }

        private IEnumerable<PerformanceTitleViewModel> GetPerformanceTitles()
           => this.data
               .Performances
               .Select(p => new PerformanceTitleViewModel
               {
                   Id = p.Id,
                   Title = p.Title
               })
               .ToList();
    }
}
