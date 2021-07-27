namespace OperaHouseTheater.Controllers.Performance
{
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Data;
    using OperaHouseTheater.Data.Models;
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

            if (this.data.Performances.FirstOrDefault(p=> p.Title == performance.Title) != null)
            {
                this.ModelState.AddModelError(nameof(performance.Title), "Performance with that title already exist.");
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

            return RedirectToAction(nameof(All));
        }

        public IActionResult All() 
        {
            var performances = this.data
                .Performances
                .OrderByDescending(x => x.Id)
                .Select(p => new PerformanceListingViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Composer = p.Composer,
                    ImageUrl = p.ImageUrl,
                    PerformanceType = p.PerformanceType.Type
                }).ToList();

            return View(performances);
        }

        public IActionResult Details(int id) 
        {
            var performance = data.Performances.FirstOrDefault(p => p.Id == id);

            if (performance == null)
            {
                return BadRequest();
            }

            var performanceData = new PerformanceDetailsViewModel
            { 
                Id = performance.Id,
                Title = performance.Title,
                Composer = performance.Composer,
                Synopsis = performance.Synopsis,
                ImageUrl = performance.ImageUrl,
                Roles = data.RolesPerformance.Where(r => r.PerformanceId == performance.Id)
                        .Select(r => new RoleListingViewModel
                        {
                            Id = r.Id,
                            RoleName = r.RoleName
                        }).ToList(),
                Events = data.Events.Where(e => e.PerformanceId == performance.Id)
                        .Select(e => new EventListingViewModel
                        {
                            Id = e.Id,
                            Date = e.Date
                        })
                        .ToList(),
                Comments = data.Comments.Where(c => c.PerformanceId == performance.Id)
                        .Select(c => new CommentListingViewModel
                        {
                            Id = c.Id,
                            Content = c.Content,
                            CreatorName = c.Member.MemberName
                        }).ToList()
            };

            return View(performanceData);
        }

        public IActionResult Delete(int id) 
        {
            var performance = data
                .Performances
                .Where(p => p.Id == id)
                .FirstOrDefault();

            if (performance == null)
            {
                return BadRequest();
            }

            var events = data.Events.Where(e => e.PerformanceId == id);

            if (events != null)
            {
                data.Events.RemoveRange(events);
            }

            var roles = data.RolesPerformance.Where(r => r.PerformanceId == id);

            if (roles != null)
            {
                data.RolesPerformance.RemoveRange(roles);
            }

            var comments = data.Comments.Where(c => c.PerformanceId == id);

            if (comments != null)
            {
                data.Comments.RemoveRange(comments);
            }

            data.Performances.Remove(performance);
            data.SaveChanges();

            return RedirectToAction(nameof(All));
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
