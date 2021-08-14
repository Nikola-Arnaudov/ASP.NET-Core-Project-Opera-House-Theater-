namespace OperaHouseTheater.Controllers.Performance
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using OperaHouseTheater.Models.Performance;
    using OperaHouseTheater.Services.Performances;
    using OperaHouseTheater.Services.Tickets;
    using OperaHouseTheater.Infrastructure;

    using static WebConstants;

    public class PerformanceController : Controller
    {
        private readonly IPerformanceService performances;
        private readonly ITicketService tickets;

        public PerformanceController(
            IPerformanceService performances, 
            ITicketService tickets)
        {
            this.performances = performances;
            this.tickets = tickets;
        }

        [Authorize]
        //only Admin
        public IActionResult Create() 
        {
            if (!User.IsAdmin())
            {
                TempData["ErrorMessage"] = "Аccess denied.";

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
                TempData["ErrorMessage"] = "Аccess denied.";

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
                TempData["ErrorMessage"] = "Аccess denied.";

                return RedirectToAction("Error", "Home");
            }

            var performance = performances.GetPerformanceById(id);

            if (performance == null)
            {
                TempData["ErrorMessage"] = "Performance with that id does not exist.";

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
                TempData["ErrorMessage"] = "Аccess denied.";

                return RedirectToAction("Error", "Home");
            }

            if (performance.PerformanceTypeId == 0)
            {
                this.ModelState.AddModelError(nameof(performance.PerformanceTypeId), "You must chose a type.");
            }

            if (!this.performances.GetPerformanceTypes().Any(t => t.Id == performance.PerformanceTypeId))
            {
                this.ModelState.AddModelError(nameof(performance.PerformanceTypeId), "This type doesn't exist.");
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
                TempData["ErrorMessage"] = "This performance doesn't exist.";

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

            if (crrPerformance == null)
            {
                TempData["ErrorMessage"] = "This performance doesn't exist.";

                return RedirectToAction("Error", "Home");
            }

            //var performanceData = new PerformanceDetailsViewModel
            //{
            //    Synopsis = crrPerformance.Synopsis,
            //    Comments = crrPerformance.Comments,
            //    Composer = crrPerformance.Composer,
            //    Events = crrPerformance.Events,
            //    ImageUrl = crrPerformance.ImageUrl,
            //    Id = crrPerformance.Id,
            //    Roles = crrPerformance.Roles,
            //    Title = crrPerformance.Title
            //};

            //return View(performanceData);
            return View(crrPerformance);
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

            var ticketsExist = this.tickets.PerformanceTicketsExists(id);

            if (ticketsExist)
            {
                TempData["ErrorMessage"] = "There are sold tickets for that performance.You can't delete it before all the events are finished!";

                return RedirectToAction("Error", "Home");
            }
            else
            {
                this.tickets.CleareExpiredTickets(id);
            }

            var performance = this.performances.Delete(id);

            if (performance == false)
            {
                TempData["ErrorMessage"] = "The performance with that id it doesn't exist.";

                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction(nameof(All));
        }
    }
}
