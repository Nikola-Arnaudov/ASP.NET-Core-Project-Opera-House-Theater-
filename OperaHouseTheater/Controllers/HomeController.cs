namespace OperaHouseTheater.Controllers
{
    using System;
    using System.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using OperaHouseTheater.Models;
    using OperaHouseTheater.Services.Home;
    using OperaHouseTheater.Services.Home.Models;

    using static WebConstants.Cache;

    public class HomeController : Controller
    {
        private readonly IHomeService home;
        private readonly IMemoryCache cache;

        public HomeController(IHomeService home, 
            IMemoryCache cache)
        {
            this.home = home;
            this.cache = cache;
        } 

        public IActionResult Index()
        {
            var indexPage = this.cache.Get<HomeServiceModel>(IndexPageCacheKey);

            if (indexPage == null)
            {
                indexPage = this.home.IndexAll();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

                this.cache.Set(IndexPageCacheKey, indexPage, cacheOptions);
            }

            return View(indexPage);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() 
        {
            var msg = TempData["ErrorMessage"] as string;

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,Message = msg});
        } 

    }
}
