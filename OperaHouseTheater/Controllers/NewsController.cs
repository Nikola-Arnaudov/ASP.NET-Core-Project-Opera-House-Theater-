namespace OperaHouseTheater.Controllers.News
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using OperaHouseTheater.Models.News;
    using OperaHouseTheater.Services.News;
    using OperaHouseTheater.Infrastructure;

    public class NewsController : Controller
    {
        private readonly INewsService news;

        public NewsController(INewsService news)
        {
            this.news = news;
        }

        public IActionResult All([FromQuery] AllNewsQueryModel query)
        {
            var queryResult = this.news.All(
                query.SearchTerm,
                query.CurrentPage,
                AllNewsQueryModel.NewsPerPage);

            query.News = queryResult.News;

            return View(query);
        }

        //[Authorize]
        ////only Admin
        //public IActionResult Add()
        //{
        //    if (!User.IsAdmin())
        //    {
        //        TempData["ErrorMessage"] = "Аccess denied.";

        //        return RedirectToAction("Error", "Home");
        //    }

        //    return View();
        //}

        //[Authorize]
        //[HttpPost]
        ////only Admin
        //public IActionResult Add(AddNewsFormModel news)
        //{
        //    if (!User.IsAdmin())
        //    {
        //        TempData["ErrorMessage"] = "Аccess denied.";

        //        return RedirectToAction("Error", "Home");
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return View(news);
        //    }

        //    this.news.Add(news.Title, news.Content, news.ImageUrl, news.VideoUrl);

        //    return RedirectToAction(nameof(All));
        //}

        public IActionResult Details(int id)
        {
            var news = this.news.GetNewsById(id);

            if (news == null)
            {
                TempData["ErrorMessage"] = "This news doesn't exist.";

                return RedirectToAction("Error", "Home");
            }

            var newsData = new NewsServiceModel
            {
                Content = news.Content,
                ImageUrl = news.NewsImageUrl,
                Title = news.Title,
                VideoUrl = news.NewsVideoUrl
            };

            return View(newsData);
        }

        //[Authorize]
        ////only Admin
        //public IActionResult Delete(int id)
        //{
        //    if (!User.IsAdmin())
        //    {
        //        TempData["ErrorMessage"] = "Аccess denied.";

        //        return RedirectToAction("Error", "Home");
        //    }

        //    var news = this.news.GetNewsById(id);

        //    if (news == null)
        //    {
        //        TempData["ErrorMessage"] = "News with this id doesn't exist.";

        //        return RedirectToAction("Error", "Home");
        //    }

        //    this.news.Delete(id);

        //    return RedirectToAction(nameof(All));
        //}
    }
}
