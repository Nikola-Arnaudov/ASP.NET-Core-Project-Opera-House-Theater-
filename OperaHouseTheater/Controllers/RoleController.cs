namespace OperaHouseTheater.Controllers.PerformanceRole
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Data;
    using OperaHouseTheater.Data.Models;
    using OperaHouseTheater.Models.Performance;
    using OperaHouseTheater.Services.Performances;
    using OperaHouseTheater.Services.Roles;

    public class RoleController : Controller
    {
        //private readonly OperaHouseTheaterDbContext data;
        private readonly IRoleService roles;
        private readonly IPerformanceService performances;

        public RoleController(/*OperaHouseTheaterDbContext data,*/
            IRoleService roles,
            IPerformanceService performances)
        { 
            //this.data = data;
            this.roles = roles;
            this.performances = performances;
        }

        [Authorize]
        //only Admin
        public IActionResult AddRole() 
            => View();
        //public IActionResult AddRole() => View(new AddRoleFormModel
        //{
        //    PerformanceTitles = this.GetPerformanceTitles()
        //});



        [HttpPost]
        [Authorize]
        //only Admin
        public IActionResult AddRole(AddRoleFormModel role)
        {
            if (!this.performances.PerformanceExistById(role.PerformanceId)/*!this.data.Performances.Any(p => p.Id == role.PerformanceId)*/)
            {
                //TODO Error message

                return BadRequest();
                //this.ModelState.AddModelError(nameof(role.PerformanceTitles), "This performance does not exist.");
            }

            if (!ModelState.IsValid)
            {
                //role.PerformanceTitles = this.GetPerformanceTitles();

                return View(role);
            }

            //var roleData = new Role
            //{
            //    RoleName = role.Name,
            //    PerformanceId = role.PerformanceId
            //};

            //this.data.RolesPerformance.Add(roleData);
            //this.data.SaveChanges();

            this.roles.Add(role.Name, role.PerformanceId);

            return Redirect($"/Performance/Details/{role.PerformanceId}");
        }

        [Authorize]
        //only Admin
        public IActionResult Delete(int id) 
        {
            var performanceId = this.roles.Delete(id);

            if (performanceId == 0)
            {
                //TODO Message

                return BadRequest();
            }

            //var performanceId = role.PerformanceId;

            //this.data.RolesPerformance.Remove(role);
            //this.data.SaveChanges();

            return Redirect($"/Performance/Details/{performanceId}");
        }

        //private IEnumerable<PerformanceTitleViewModel> GetPerformanceTitles()
        //   => this.data
        //       .Performances
        //       .Select(p => new PerformanceTitleViewModel
        //       {
        //           Id = p.Id,
        //           Title = p.Title
        //       })
        //       .ToList();
    }
}
