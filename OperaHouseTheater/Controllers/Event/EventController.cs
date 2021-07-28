namespace OperaHouseTheater.Controllers.Event
{
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Data;
    using OperaHouseTheater.Data.Models;
    using OperaHouseTheater.Models.Event;
    using OperaHouseTheater.Models.Performance;
    using System;
    using System.Collections.Generic;
    using System.Linq;


    public class EventController : Controller
    {
        private readonly OperaHouseTheaterDbContext data;

        public EventController(OperaHouseTheaterDbContext data)
        {
            this.data = data;
        }

        public IActionResult Create() => View(new CreateEventFormModel
        {
            PerformanceTitles = this.GetPerformanceTitles()
        });

        [HttpPost]
        public IActionResult Create(CreateEventFormModel eventInput)
        {
            if (!this.data.Performances.Any(p => p.Id == eventInput.PerformanceId))
            {
                this.ModelState.AddModelError(nameof(eventInput.PerformanceId), "This performance does not exist.");
            }

            if (DateTime.Compare(eventInput.Date, DateTime.Now) <= 0)
            {
                this.ModelState.AddModelError(nameof(eventInput.Date), "The date has already passed.");
            }

            if (!ModelState.IsValid)
            {
                eventInput.PerformanceTitles = this.GetPerformanceTitles();

                return View(eventInput);
            }

            var eventData = new Event
            {
                PerformanceId = eventInput.PerformanceId,
                Date = eventInput.Date,
                TicketPrice = eventInput.TicketPrice,
                Performance = data.Performances.FirstOrDefault(x => x.Id == eventInput.PerformanceId)
            };

            this.data.Events.Add(eventData);
            this.data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult All()
        {
            var events = this.data
                .Events
                .OrderBy(e => e.Date)
                .Select(e => new EventsListingViewModel()
                {
                    Id = e.Id,
                    EventName = e.Performance.Title,
                    Composer = e.Performance.Composer,
                    PerformanceType = e.Performance.PerformanceType.Type,
                    Date = e.Date
                })
                .ToList();

            return View(events);
        }

        public IActionResult Details(int id) 
        {
            var crrEvent = this.data
                .Events
                .FirstOrDefault(x => x.Id == id);

            var crrPerformance = this.data.Performances.FirstOrDefault(x => x.Id == crrEvent.PerformanceId);

            //TODO: view with error message

            if (crrEvent == null)
            {
                return BadRequest();
            }

            var eventData = new EventDetailsViewModel
            {
                Id = crrEvent.Id,
                Title = crrPerformance.Title,
                Composer = crrPerformance.Composer,
                Synopsis = crrPerformance.Synopsis,
                ImageUrl = crrPerformance.ImageUrl,
                Date = crrEvent.Date,
                PerformanceId = crrPerformance.Id,
                EventRoles = data.EventRoles.Where(e => e.EventId == crrEvent.Id)
                        .Select(r => new EventRoleListinViewModel
                        {
                            Id = r.Id,
                            RoleName = r.Role.RoleName,
                            EmployeeFirstName = r.Employee.FirstName,
                            EmployeeLastName = r.Employee.LastName,
                            ImageUrl = r.Employee.ImageUrl

                        }).ToList(),
            };

            return View(eventData);
        }

        public IActionResult Delete(int id)
        {
            var crrEvent = this.data.Events
                .FirstOrDefault(x => x.Id == id);

            //TODO: if news is null...
            if (crrEvent == null)
            {
                return BadRequest();
            }

            var eventRoles = this.data
                .EventRoles
                .Where(er => er.EventId == id)
                .ToList();

            this.data.EventRoles.RemoveRange(eventRoles);
            this.data.SaveChanges();

            this.data.Events.Remove(crrEvent);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        public IActionResult SetRole(string ids) => View(new SetEventRoleFormModel
        {
            //PerformanceId = id,
            //Employees = this.GetEmployees(),
            //Roles = this.GetRolesNames(id)
        });

        [HttpPost]
        public IActionResult SetRole(SetEventRoleFormModel role) 
        {
            //TODO: Return Error message if there is no performance with this id
            if (!this.data.Performances.Any(x=> x.Id == role.PerformanceId))
            {
                
            }

            if (!this.data.RolesPerformance.Any(r => r.Id == role.RoleId))
            {
                this.ModelState.AddModelError(nameof(role.RoleId), "This role does not exist.");
            }

            if (!ModelState.IsValid)
            {
                role.Employees = this.GetEmployees();
                role.Roles = this.GetRolesNames(role.PerformanceId);

                return View(role);
            }

            var roleData = new EventRole
            {
                RoleId = role.RoleId,
                EmployeeId = role.EmployeeId
            };

            this.data.EventRoles.Add(roleData);
            this.data.SaveChanges();

            return Redirect($"/Performance/Details/{role.PerformanceId}");
        }

        private IEnumerable<EventEmployeeModel> GetEmployees()
            => this.data.Employees
                .Select(e => new EventEmployeeModel
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Category = e.Category.CategoryName
                });

        private IEnumerable<EventRoleModel> GetRolesNames(int id)
            => this.data.RolesPerformance
                .Where(r => r.PerformanceId == id)
                .Select(r => new EventRoleModel
                {
                    Id = r.Id,
                    RoleName = r.RoleName,
                });

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
