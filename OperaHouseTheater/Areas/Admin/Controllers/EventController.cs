namespace OperaHouseTheater.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Services.Admins;
    using OperaHouseTheater.Services.Events;
    using OperaHouseTheater.Infrastructure;
    using OperaHouseTheater.Models.Event;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using static AdminConstants;

    [Area(AreaName)]
    [Authorize(Roles = AdministratorRoleName)]
    public class EventController : Controller
    {
        private readonly IEventService events;

        public EventController(IEventService events) 
            => this.events = events;

        [Authorize]
        //only Admin
        public IActionResult Create()
        {
            if (!User.IsAdmin())
            {
                TempData["ErrorMessage"] = "Аccess denied.";

                return RedirectToAction("Error", "Home", new { area = "" });
            }

            return View(new CreateEventFormModel
            {
                PerformanceTitles = this.events.GetPerformanceTitles()
            });
        }

        [Authorize]
        [HttpPost]
        //only Admin
        public IActionResult Create(CreateEventFormModel eventInput)
        {
            if (!User.IsAdmin())
            {
                TempData["ErrorMessage"] = "Аccess denied.";

                return RedirectToAction("Error", "Home", new { area = "" });
            }

            if (!this.events.GetPerformanceTitles().Any(p => p.Id == eventInput.PerformanceId))
            {
                this.ModelState.AddModelError(nameof(eventInput.PerformanceId), "This performance doesn't exist.");
            }

            if (DateTime.Compare(eventInput.Date, DateTime.Now) <= 0)
            {
                this.ModelState.AddModelError(nameof(eventInput.Date), "The date has already passed.");
            }

            if (!ModelState.IsValid)
            {
                eventInput.PerformanceTitles = this.events.GetPerformanceTitles();

                return View(eventInput);
            }

            var eventData = events.Create(
                eventInput.PerformanceId,
                eventInput.Date,
                eventInput.TicketPrice);

            return Redirect($"/Event/Details/{eventData}");
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

            var eventExist = this.events.EventExist(id);

            if (eventExist == false)
            {
                TempData["ErrorMessage"] = "Event with that id it doesn't exist.";

                return RedirectToAction("Error", "Home", new { area = "" });
            }

            //TODO : error if the event.date <= date.now
            var ticketSoldForEvent = this.events.EventTicketsExist(id);

            if (ticketSoldForEvent)
            {
                TempData["ErrorMessage"] = "There are sold tickets for this event. You can't delete it before it's over.";

                return RedirectToAction("Error", "Home", new { area = "" });
            }

            this.events.Delete(id);

            return RedirectToAction("All", "Event", new { area = "" });
        }

        [Authorize]
        //only Admin
        public IActionResult SetRole(int performanceId)
        {
            if (!User.IsAdmin())
            {
                TempData["ErrorMessage"] = "Аccess denied.";

                return RedirectToAction("Error", "Home", new { area = "" });
            }

            return View(new SetEventRoleFormModel
            {
                Employees = this.events.GetEmployees(),
                Roles = this.events.GetRolesNames(performanceId)
            });
        }

        [Authorize]
        [HttpPost]
        //only Admin
        public IActionResult SetRole(SetEventRoleFormModel role)
        {
            if (!User.IsAdmin())
            {
                TempData["ErrorMessage"] = "Аccess denied.";

                return RedirectToAction("Error", "Home", new { area = "" });
            }

            if (!this.events.GetPerformanceTitles().Any(x => x.Id == role.PerformanceId))
            {
                this.ModelState.AddModelError(nameof(role.PerformanceId), "This performance doesn't exist.");
            }

            if (!this.events.RolesPerformanceExist(role.RoleId))
            {
                this.ModelState.AddModelError(nameof(role.RoleId), "This role doesn't exist.");
            }

            if (!ModelState.IsValid)
            {
                role.Employees = this.events.GetEmployees();
                role.Roles = this.events.GetRolesNames(role.PerformanceId);

                return View(role);
            }

            this.events.SetRole(role.RoleId, role.EmployeeId, role.EventId);

            return Redirect($"/Event/Details/{role.EventId}");
        }

        [Authorize]
        //only Admin
        public IActionResult DeleteEventRole(int id)
        {
            if (!User.IsAdmin())
            {
                TempData["ErrorMessage"] = "Аccess denied.";

                return RedirectToAction("Error", "Home", new { area = "" });
            }

            var crrEvent = this.events.DeleteEventRole(id);

            if (crrEvent == 0)
            {
                TempData["ErrorMessage"] = "This event role doesn't exist.";

                return RedirectToAction("Error", "Home", new { area = "" });
            }

            return Redirect($"/Event/Details/{crrEvent}");
        }

    }
}
