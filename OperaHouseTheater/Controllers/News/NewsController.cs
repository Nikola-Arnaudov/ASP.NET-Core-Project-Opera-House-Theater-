namespace OperaHouseTheater.Controllers.News
{
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Models.News;
    using Data.Models;
    using OperaHouseTheater.Data;
    using System.Linq;

    public class NewsController : Controller
    {
        private readonly OperaHouseTheaterDbContext data;

        public NewsController(OperaHouseTheaterDbContext data)
            => this.data = data;

        public IActionResult All()
        {
            var news = this.data
                .News
                .OrderByDescending(n => n.Id)
                .Select(n => new NewsListingViewModel()
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    ImageUrl = n.NewsImageUrl,
                    VideoUrl = n.NewsVideoUrl
                })
                .ToList();

            return View(news);
        }

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
                NewsImageUrl = news.ImageUrl,
                NewsVideoUrl = news.VideoUrl ?? null
            };

            this.data.News.Add(newsData);

            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        public IActionResult Details(int id) 
        {
            var news = data
                .News
                .FirstOrDefault(x => x.Id == id);

            var newsData = new NewsListingViewModel
            {
                Content = news.Content,
                ImageUrl = news.NewsImageUrl,
                Title = news.Title,
                VideoUrl = news.NewsVideoUrl
            };

            return View(newsData);
        }

        public IActionResult Delete(int id) 
        {
            var news = data.News
                .FirstOrDefault(x => x.Id == id);

            this.data.News.Remove(news);

            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }
    }
}
