namespace OperaHouseTheater.Controllers.Performance
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Data;
    using OperaHouseTheater.Data.Models;
    using OperaHouseTheater.Models.Performance;
    using OperaHouseTheater.Services.Performances;
    using OperaHouseTheater.Services.Tickets;
    using System.Collections.Generic;
    using System.Linq;

    public class PerformanceController : Controller
    {
        //private readonly OperaHouseTheaterDbContext data;
        private readonly IPerformanceService performances;
        private readonly ITicketService tickets;

        public PerformanceController(/*OperaHouseTheaterDbContext data,*/ 
            IPerformanceService performances, 
            ITicketService tickets)
        {
            //this.data = data;
            this.performances = performances;
            this.tickets = tickets;
        }

        [Authorize]
        //only Admin
        public IActionResult Create() => View(new AddPerformanceFormModel
        {
            PerformanceTypes = this.performances.GetPerformanceTypes()
        });

        [HttpPost]
        [Authorize]
        //only Admin
        public IActionResult Create(AddPerformanceFormModel performance) 
        {
            if (performance.PerformanceTypeId == 0)
            {
                this.ModelState.AddModelError(nameof(performance.PerformanceTypeId), "You must chose a type.");
            }

            //if (!this.data.PerformanceTypes.Any(t=> t.Id == performance.PerformanceTypeId))
            //{
            //    this.ModelState.AddModelError(nameof(performance.PerformanceTypeId), "This type does not exist.");
            //}
            if (!this.performances.GetPerformanceTypes().Any(t => t.Id == performance.PerformanceTypeId))
            {
                this.ModelState.AddModelError(nameof(performance.PerformanceTypeId), "This type does not exist.");
            }

            //if (this.data.Performances.FirstOrDefault(p=> p.Title == performance.Title) != null)
            //{
            //    this.ModelState.AddModelError(nameof(performance.Title), "Performance with that title already exist.");
            //}
            if (this.performances.PerformanceTitleExist(performance.Title))
            {
                this.ModelState.AddModelError(nameof(performance.Title), "Performance with that title already exist.");
            }

            if (!ModelState.IsValid)
            {
                performance.PerformanceTypes = this.performances.GetPerformanceTypes();

                return View(performance);
            }

            //var newPerformance = new Performance
            //{
            //    Title = performance.Title,
            //    Composer = performance.Composer,
            //    Synopsis = performance.Synopsis,
            //    ImageUrl = performance.ImageUrl,
            //    PerformanceTypeId = performance.PerformanceTypeId
            //};

            //this.data.Add(newPerformance);
            //this.data.SaveChanges();

            this.performances.Add(performance.Title, 
                performance.Composer, 
                performance.Synopsis, 
                performance.ImageUrl,
                performance.PerformanceTypeId);

            return RedirectToAction(nameof(All));
        }

        public IActionResult All([FromQuery]AllPerformancesQueryModel query) 
        {
            //var performanceQuery = this.data.Performances.AsQueryable();

            //if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            //{
            //    performanceQuery = performanceQuery.Where(n =>
            //        n.Title.ToLower().Contains(query.SearchTerm.ToLower())
            //        || n.Composer.ToLower().Contains(query.SearchTerm.ToLower())
            //        || n.PerformanceType.Type.ToLower().Contains(query.SearchTerm.ToLower()));
            //}

            //if (!string.IsNullOrWhiteSpace(query.Type))
            //{
            //    performanceQuery = performanceQuery.Where(p => p.PerformanceType.Type == query.Type);
            //}

            //var performances = performanceQuery
            //    .OrderByDescending(x => x.Id)
            //    .Select(p => new PerformanceListingViewModel
            //    {
            //        Id = p.Id,
            //        Title = p.Title,
            //        Composer = p.Composer,
            //        ImageUrl = p.ImageUrl,
            //        PerformanceType = p.PerformanceType.Type
            //    }).ToList();

            //var types = this.data
            //    .PerformanceTypes
            //    .Select(t => t.Type)
            //    .ToList();

            var queryResult = this.performances.All(query.SearchTerm, query.Type);

            query.Types = queryResult.Types;
            query.Performances = queryResult.Performances;

            return View(query);
        }

        public IActionResult Details(int id) 
        {
            var crrPerformance = this.performances.Details(id);

            //TODO Error Message
            if (crrPerformance == null)
            {
                return BadRequest();
            }

            //var performanceData = new PerformanceDetailsViewModel
            //{ 
            //    Id = performance.Id,
            //    Title = performance.Title,
            //    Composer = performance.Composer,
            //    Synopsis = performance.Synopsis,
            //    ImageUrl = performance.ImageUrl,
            //    Roles = this.data.RolesPerformance
            //            .Where(r => r.PerformanceId == performance.Id)
            //            .Select(r => new RoleListingViewModel
            //            {
            //                Id = r.Id,
            //                RoleName = r.RoleName
            //            }).ToList(),
            //    Events = this.data.Events
            //            .Where(e => e.PerformanceId == performance.Id)
            //            .Select(e => new EventListingViewModel
            //            {
            //                Id = e.Id,
            //                Date = e.Date
            //            })
            //            .ToList(),
            //    Comments = this.data.Comments
            //            .OrderByDescending(x=> x.Id)
            //            .Where(c => c.PerformanceId == performance.Id)
            //            .Select(c => new CommentListingViewModel
            //            {
            //                Id = c.Id,
            //                Content = c.Content,
            //                CreatorName = c.Member.MemberName
            //            }).ToList()
            //};

            return View(crrPerformance);
        }

        [Authorize]
        //only Admin
        public IActionResult Delete(int id) 
        {
            //var performance = data
            //    .Performances
            //    .Where(p => p.Id == id)
            //    .FirstOrDefault();

            //if (performance == null)
            //{
            //    return BadRequest();
            //}

            //var events = data.Events.Where(e => e.PerformanceId == id);

            //if (events != null)
            //{
            //    data.Events.RemoveRange(events);
            //}

            //var roles = data.RolesPerformance.Where(r => r.PerformanceId == id);

            //if (roles != null)
            //{
            //    data.RolesPerformance.RemoveRange(roles);
            //}

            //var comments = data.Comments.Where(c => c.PerformanceId == id);

            //if (comments != null)
            //{
            //    data.Comments.RemoveRange(comments);
            //}

            //data.Performances.Remove(performance);
            //data.SaveChanges();

            var ticketsExist = this.tickets.TicketsExists(id);

            if (ticketsExist)
            {
                //TODO Message: There are sold tickets for that performance.You can't delete it 
                // before all the events are finished!

                return BadRequest();
            }
            else
            {
                this.tickets.CleareExpiredTickets(id);
            }

            var performance = this.performances.Delete(id);

            if (performance == false)
            {
                //TODO Error message

                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        //private IEnumerable<PerformanceTypeViewModel> GetPerformanceTypes()
        //    => this.data
        //        .PerformanceTypes
        //        .Select(p => new PerformanceTypeViewModel
        //        {
        //            Id = p.Id,
        //            TypeName = p.Type
        //        })
        //        .ToList();
    }
}
