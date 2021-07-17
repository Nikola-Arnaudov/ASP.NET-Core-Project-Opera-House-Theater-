namespace OperaHouseTheater.Controllers.News
{
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Models.News;
    using Data.Models;
    using OperaHouseTheater.Data;

    public class NewsController : Controller
    {
        private readonly OperaHouseTheaterDbContext data;

        public NewsController(OperaHouseTheaterDbContext data)
            => this.data = data;

        public IActionResult Add() => View();

        [HttpPost]
        public IActionResult Add(AddNewsFormModel news) 
        {
            if (!ModelState.IsValid)
            {
                return View(news);
            }

            var newsData = new News
            {
                Title = news.Title,
                Content = news.Content,
                NewsPictureUrl = news.PictureUrl,
                NewsVideoUrl = news.VideoUrl ?? null
            };

            this.data.News.Add(newsData);
            
            this.data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
