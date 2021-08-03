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

        public IActionResult All([FromQuery]AllNewsQueryModel query)
        {
            var newsQuery = this.data.News.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                newsQuery = newsQuery.Where(n =>
                    n.Title.ToLower().Contains(query.SearchTerm.ToLower())
                    || n.Content.ToLower().Contains(query.SearchTerm.ToLower()));
            }

            var news = newsQuery
                .OrderByDescending(n => n.Id)
                .Skip((query.CurrentPage - 1) * AllNewsQueryModel.NewsPerPage)
                .Take(AllNewsQueryModel.NewsPerPage)
                .Select(n => new NewsListingViewModel()
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    ImageUrl = n.NewsImageUrl,
                    VideoUrl = n.NewsVideoUrl
                })
                .ToList();

            query.News = news;
            query.NewsCount = newsQuery.Count();

            return View(query);
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

            //TODO: if news is null...

                
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
            var news = this.data.News
                .FirstOrDefault(x => x.Id == id);

            //TODO: if news is null...
            if (news == null)
            {
                return BadRequest();
            }

            this.data.News.Remove(news);

            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }
    }
}
