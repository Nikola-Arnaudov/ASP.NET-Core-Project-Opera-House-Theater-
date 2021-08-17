namespace OperaHouseTheater.Controllers.Performance
{
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Models.Performance;
    using OperaHouseTheater.Services.Performances;

    using static WebConstants;

    public class PerformanceController : Controller
    {
        private readonly IPerformanceService performances;

        public PerformanceController(
            IPerformanceService performances) 
            => this.performances = performances;

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

            return View(crrPerformance);
        }
    }
}
