namespace OperaHouseTheater.Controllers.News
{
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Models.News;
    using OperaHouseTheater.Services.News;
    using Microsoft.AspNetCore.Authorization;
    using OperaHouseTheater.Services.Admins;
    using OperaHouseTheater.Infrastructure;

    public class NewsController : Controller
    {
        private readonly INewsService news;
        private readonly IAdminService admins;

        public NewsController(INewsService news,
            IAdminService admins)
        {
            this.news = news;
            this.admins = admins;
        }

        public IActionResult All([FromQuery]AllNewsQueryModel query)
        {
            var queryResult = this.news.All(
                query.SearchTerm,
                query.CurrentPage,
                AllNewsQueryModel.NewsPerPage);

            query.News = queryResult.News;

            return View(query);
        }

        [Authorize]
        //only Admin
        public IActionResult Add() 
        {
            if (!this.admins.UserIsAdmin(this.User.GetId()))
            {
                //TODO Error message
                return BadRequest();
            }

            return View();
        }

        [Authorize]
        [HttpPost]
        //only Admin
        public IActionResult Add(AddNewsFormModel news)
        {
            if (!this.admins.UserIsAdmin(this.User.GetId()))
            {
                //TODO Error message
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(news);
            }

            this.news.Add(news.Title, news.Content, news.ImageUrl, news.VideoUrl);

            return RedirectToAction(nameof(All));
        }

        public IActionResult Details(int id) 
        {
            var news = this.news.GetNewsById(id);

            //TODO: if news is null...
            if (news == null)
            {
                //TODO Error message

                return BadRequest();
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

        [Authorize]
        //only Admin
        public IActionResult Delete(int id) 
        {
            if (!this.admins.UserIsAdmin(this.User.GetId()))
            {
                //TODO Error message
                return BadRequest();
            }

            var news = this.news.GetNewsById(id);

            //TODO: if news is null...
            if (news == null)
            {
                return BadRequest();
            }

            this.news.Delete(id);

            return RedirectToAction(nameof(All));
        }
    }
}
