namespace OperaHouseTheater.Controllers
{
    using System.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Models;
    using OperaHouseTheater.Services.Home;

    public class HomeController : Controller
    {
        private readonly IHomeService home;

        public HomeController(IHomeService home) 
            => this.home = home;


        public IActionResult Index()
        {
            var indexPageData = this.home.IndexAll();

            return View(indexPageData);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() 
        {
            var msg = TempData["ErrorMessage"] as string;

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,Message = msg});
        } 

    }
}
