namespace OperaHouseTheater.Controllers.News
{
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Models.News;
    using OperaHouseTheater.Services.News;

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
    }
}
