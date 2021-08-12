namespace OperaHouseTheater.Controllers.Performance
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Models.Performance;
    using OperaHouseTheater.Services.Performances;
    using OperaHouseTheater.Services.Tickets;
    using OperaHouseTheater.Infrastructure;
    using System.Linq;

    using static WebConstants;

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
        public IActionResult Create() 
        {
            if (!User.IsAdmin())
            {
                return RedirectToAction("Error", "Home");
            }

            return View(new PerformanceFormModel
            {
                PerformanceTypes = this.performances.GetPerformanceTypes()
            });
        }

        [HttpPost]
        [Authorize]
        //only Admin
        public IActionResult Create(PerformanceFormModel performance) 
        {
            if (!User.IsAdmin())
            {
                return RedirectToAction("Error", "Home");
            }

            if (performance.PerformanceTypeId == 0)
            {
                this.ModelState.AddModelError(nameof(performance.PerformanceTypeId), "You must chose a type.");
            }

            if (!this.performances.GetPerformanceTypes().Any(t => t.Id == performance.PerformanceTypeId))
            {
                this.ModelState.AddModelError(nameof(performance.PerformanceTypeId), "This type does not exist.");
            }

            if (this.performances.PerformanceTitleExist(performance.Title))
            {
                this.ModelState.AddModelError(nameof(performance.Title), "Performance with that title already exist.");
            }

            if (!ModelState.IsValid)
            {
                performance.PerformanceTypes = this.performances.GetPerformanceTypes();

                return View(performance);
            }

            this.performances.Add(performance.Title, 
                performance.Composer, 
                performance.Synopsis, 
                performance.ImageUrl,
                performance.PerformanceTypeId);

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            if (!User.IsAdmin())
            {
                return RedirectToAction("Error", "Home");
            }

            var performance = performances.GetPerformanceById(id);

            if (performance == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(new PerformanceFormModel
            {
                Title = performance.Title,
                Composer = performance.Composer,
                ImageUrl = performance.ImageUrl,
                Synopsis = performance.Synopsis,
                PerformanceTypeId = performance.PerformanceTypeId,
                PerformanceTypes = this.performances.GetPerformanceTypes()
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int id,PerformanceFormModel performance) 
        {
            if (!User.IsAdmin())
            {
                return RedirectToAction("Error", "Home");
            }

            if (performance.PerformanceTypeId == 0)
            {
                this.ModelState.AddModelError(nameof(performance.PerformanceTypeId), "You must chose a type.");
            }

            if (!this.performances.GetPerformanceTypes().Any(t => t.Id == performance.PerformanceTypeId))
            {
                this.ModelState.AddModelError(nameof(performance.PerformanceTypeId), "This type does not exist.");
            }

            if (!ModelState.IsValid)
            {
                performance.PerformanceTypes = this.performances.GetPerformanceTypes();

                return View(performance);
            }

            var performanceIsEdited = this.performances.Edit(
                id,
                performance.Title,
                performance.Composer,
                performance.Synopsis,
                performance.ImageUrl,
                performance.PerformanceTypeId);

            if (!performanceIsEdited)
            {
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult All([FromQuery]AllPerformancesQueryModel query) 
        {
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

            return View(crrPerformance);
        }

        [Authorize]
        //only Admin
        public IActionResult Delete(int id) 
        {
            

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
    }
}
