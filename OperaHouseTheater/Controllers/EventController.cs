namespace OperaHouseTheater.Controllers.Event
{
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Data;
    using OperaHouseTheater.Models.Event;
    using OperaHouseTheater.Services.Events;
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;

    using OperaHouseTheater.Infrastructure;

    public class EventController : Controller
    {
        private readonly IEventService events;
        //private readonly OperaHouseTheaterDbContext data;

        public EventController(IEventService events/*OperaHouseTheaterDbContext data*/)
        {
            this.events = events;
            //this.data = data;
        }

        [Authorize]
        public IActionResult Create() 
        {
            //if (!ThisUserIsAdmin())
            //{
            //    //TODO Error message

            //    return BadRequest();
            //}

            return View(new CreateEventFormModel
            {
                PerformanceTitles = this.events.GetPerformanceTitles()
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CreateEventFormModel eventInput)
        {
            //if (!ThisUserIsAdmin())
            //{
            //    //TODO Error message

            //    return BadRequest();
            //}

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

                return BadRequest();
            }

            return View(eventData);
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            //if (!ThisUserIsAdmin())
            //{
            //    //TODO Error message

            //    return BadRequest();
            //}

            var eventExist = this.events.Delete(id);

            if (eventExist == false)
            {
                //TODO Error message

                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult SetRole(int performanceId) 
        {
            //if (!ThisUserIsAdmin())
            //{
            //    //TODO Error message

            //    return BadRequest();
            //}

            return View(new SetEventRoleFormModel
            {
                Employees = this.events.GetEmployees(),
                Roles = this.events.GetRolesNames(performanceId)
            });
        } 

        [Authorize]
        [HttpPost]
        public IActionResult SetRole(SetEventRoleFormModel role) 
        {
            //if (!ThisUserIsAdmin())
            //{
            //    //TODO Error message

            //    return BadRequest();
            //}

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

            //var roledataroleId = role.RoleId;
            //var roledatapergormanceeId = role.PerformanceId;
            //var roledataEventId = role.EventId;

            //var roleData = new EventRole
            //{
            //    RoleId = role.RoleId,
            //    EmployeeId = role.EmployeeId,
            //    EventId = role.EventId,
            //};

            //this.data.EventRoles.Add(roleData);
            //this.data.SaveChanges();

            return Redirect($"/Event/Details/{role.EventId}");
        }

        [Authorize]
        public IActionResult DeleteEventRole(int id)
        {
            //if (!ThisUserIsAdmin())
            //{
            //    //TODO Error message

            //    return BadRequest();
            //}

            var crrEvent = this.events.DeleteEventRole(id);

            //TODO Message

            if (crrEvent == 0)
            {
                return BadRequest();
            }

            return Redirect($"/Event/Details/{crrEvent}");
        }



        //private IEnumerable<EventEmployeeModel> GetEmployees()
        //    => this.data.Employees
        //        .Select(e => new EventEmployeeModel
        //        {
        //            Id = e.Id,
        //            FirstName = e.FirstName,
        //            LastName = e.LastName,
        //            Category = e.Category.CategoryName
        //        });

        //private IEnumerable<EventRoleModel> GetRolesNames(int id)
        //    => this.data.RolesPerformance
        //        .Where(r => r.PerformanceId == id)
        //        .Select(r => new EventRoleModel
        //        {
        //            Id = r.Id,
        //            RoleName = r.RoleName,
        //        });

        //private IEnumerable<PerformanceTitleViewModel> GetPerformanceTitles()
        //   => this.data
        //       .Performances
        //       .Select(p => new PerformanceTitleViewModel
        //       {
        //           Id = p.Id,
        //           Title = p.Title
        //       })
        //       .ToList();

        private bool ThisUserIsAdmin()
            => this.events.UserIsAdmin(this.User.GetId());
    }
}
