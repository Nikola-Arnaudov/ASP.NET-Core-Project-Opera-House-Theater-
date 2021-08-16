namespace OperaHouseTheater.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Services.News;
    using OperaHouseTheater.Infrastructure;
    using OperaHouseTheater.Models.News;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using static AdminConstants;

    [Area(AreaName)]
    [Authorize(Roles = AdministratorRoleName)]
    public class NewsController : Controller
    {
        private readonly INewsService news;

        public NewsController(INewsService news)
        {
            this.news = news;
        }

        [Authorize]
        //only Admin
        public IActionResult Add()
        {
            if (!User.IsAdmin())
            {
                TempData["ErrorMessage"] = "Аccess denied.";

                return RedirectToAction("Error", "Home", new { area = "" });
            }

            return View();
        }

        [Authorize]
        [HttpPost]
        //only Admin
        public IActionResult Add(AddNewsFormModel news)
        {
            if (!User.IsAdmin())
            {
                TempData["ErrorMessage"] = "Аccess denied.";

                return RedirectToAction("Error", "Home", new { area = "" });
            }

            if (!ModelState.IsValid)
            {
                return View(news);
            }

            this.news.Add(news.Title, news.Content, news.ImageUrl, news.VideoUrl);

            return RedirectToAction("All", "News", new { area = "" });
        }

        [Authorize]
        //only Admin
        public IActionResult Delete(int id)
        {
            if (!User.IsAdmin())
            {
                TempData["ErrorMessage"] = "Аccess denied.";

                return RedirectToAction("Error", "Home", new { area = "" });
            }

            var news = this.news.GetNewsById(id);

            if (news == null)
            {
                TempData["ErrorMessage"] = "News with this id doesn't exist.";

                return RedirectToAction("Error", "Home", new { area = "" });
            }

            this.news.Delete(id);

            return RedirectToAction("All", "News", new { area = "" });
        }

    }
}
