namespace OperaHouseTheater.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Data;
    using OperaHouseTheater.Models;
    using OperaHouseTheater.Models.Event;
    using OperaHouseTheater.Models.Home;
    using OperaHouseTheater.Models.News;
    using OperaHouseTheater.Services.News;
    using System.Diagnostics;
    using System.Linq;

    public class HomeController : Controller
    {
        private readonly OperaHouseTheaterDbContext data;

        public HomeController(OperaHouseTheaterDbContext data)
            => this.data = data;


        public IActionResult Index()
        {

            var indexPageData = new HomeViewModel()
            {
                Events = this.data
                        .Events
                        .OrderBy(x => x.Date)
                        .Select(e => new EventsListingViewModel
                        {
                            Id = e.Id,
                            EventName = e.Performance.Title,
                            Composer = e.Performance.Composer,
                            Date = e.Date,
                            PerformanceType = e.Performance.PerformanceType.Type,
                            ImageUrl = e.Performance.ImageUrl
                        })
                        .Take(3)
                        .ToList(),
                News = this.data
                        .News
                        .OrderByDescending(x => x.Id)
                        .Select(n => new NewsServiceModel
                        {
                            Id = n.Id,
                            Title = n.Title,
                            Content = n.Content,
                            ImageUrl = n.NewsImageUrl,
                            VideoUrl = n.NewsVideoUrl,
                        })
                        .Take(3)
                        .ToList()
            };

            return View(indexPageData);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    }
}
