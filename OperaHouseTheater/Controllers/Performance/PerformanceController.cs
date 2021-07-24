namespace OperaHouseTheater.Controllers.Performance
{
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Data;
    using OperaHouseTheater.Data.Models;
    using OperaHouseTheater.Models.Performacne;
    using OperaHouseTheater.Models.Performance;
    using System.Collections.Generic;
    using System.Linq;


    public class PerformanceController : Controller
    {
        private readonly OperaHouseTheaterDbContext data;

        public PerformanceController(OperaHouseTheaterDbContext data)
            => this.data = data;

        public IActionResult Create() => View(new AddPerformanceFormModel
        {
            PerformanceTypes = this.GetPerformanceTypes()
        });

        [HttpPost]
        public IActionResult Create(AddPerformanceFormModel performance) 
        {
            if (!this.data.PerformanceTypes.Any(t=> t.Id == performance.PerformanceTypeId))
            {
                this.ModelState.AddModelError(nameof(performance.PerformanceTypeId), "This type does not exist.");
            }

            if (!ModelState.IsValid)
            {
                performance.PerformanceTypes = this.GetPerformanceTypes();

                return View(performance);
            }

            var newPerformance = new Performance
            {
                Title = performance.Title,
                Composer = performance.Composer,
                Synopsis = performance.Synopsis,
                ImageUrl = performance.ImageUrl,
                PerformanceTypeId = performance.PerformanceTypeId
            };

            this.data.Add(newPerformance);
            this.data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }


        private IEnumerable<PerformanceTypeViewModel> GetPerformanceTypes()
            => this.data
                .PerformanceTypes
                .Select(p => new PerformanceTypeViewModel
                {
                    Id = p.Id,
                    TypeName = p.Type
                })
                .ToList();

    }
}
