﻿namespace OperaHouseTheater.Controllers.Event
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using OperaHouseTheater.Models.Event;
    using OperaHouseTheater.Services.Events;
    using OperaHouseTheater.Services.Admins;
    using OperaHouseTheater.Infrastructure;
    
    using static WebConstants;

    public class EventController : Controller
    {
        private readonly IEventService events;
        private readonly IAdminService admins;

        public EventController( IEventService events,
            IAdminService admins)
        {
            this.events = events;
            this.admins = admins;
        }

        [Authorize]
        //only Admin
        public IActionResult Create() 
        {
            if (!User.IsAdmin())
            {
                //TODO Error message

                return RedirectToAction("Error", "Home");
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
                //TODO Error message

                return RedirectToAction("Error", "Home");
            }

            if (!this.events.GetPerformanceTitles().Any(p => p.Id == eventInput.PerformanceId))
            {
                this.ModelState.AddModelError(nameof(eventInput.PerformanceId), "This performance does not exist.");
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

        public IActionResult All([FromQuery]AllEventsQueryModel query)
        {
            var queryResult = this.events.All(
                query.SearchTerm,
                query.Type, 
                query.CurrentPage,
                AllEventsQueryModel.EventsPerPage);

            query.Types = queryResult.Types;
            query.Events = queryResult.Events;
            query.EventsCount = queryResult.EventsCount;

            return View(query);
        }

        public IActionResult Details(int id) 
        {
            var eventData = this.events.Details(id);

            if (eventData == null)
            {
                //TODO Error message

                return RedirectToAction("Error", "Home");
            }

            return View(eventData);
        }

        [Authorize]
        //only Admin
        public IActionResult Delete(int id)
        {
            if (!User.IsAdmin())
            {
                //TODO Error message

                return RedirectToAction("Error", "Home");
            }

            var eventExist = this.events.Delete(id);

            if (eventExist == false)
            {
                //TODO Error message

                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        //only Admin
        public IActionResult SetRole(int performanceId) 
        {
            if (!User.IsAdmin())
            {
                //TODO Error message

                return RedirectToAction("Error", "Home");
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
                //TODO Error message

                return RedirectToAction("Error", "Home");
            }

            if (!this.events.GetPerformanceTitles().Any(x => x.Id == role.PerformanceId))
            {
                this.ModelState.AddModelError(nameof(role.PerformanceId), "This Performance does not exist.");
            }

            if (!this.events.RolesPerformanceExist(role.RoleId))
            {
                this.ModelState.AddModelError(nameof(role.RoleId), "This role does not exist.");
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
                //TODO Error message

                return RedirectToAction("Error", "Home");
            }

            var crrEvent = this.events.DeleteEventRole(id);

            //TODO Message

            if (crrEvent == 0)
            {
                return RedirectToAction("Error", "Home");
            }

            return Redirect($"/Event/Details/{crrEvent}");
        }
    }
}
