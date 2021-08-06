namespace OperaHouseTheater.Services.News
{
    using System.Linq;
    using OperaHouseTheater.Data;
    using OperaHouseTheater.Data.Models;


    public class NewsService : INewsService
    {
        private readonly OperaHouseTheaterDbContext data;

        public NewsService(OperaHouseTheaterDbContext data) 
            => this.data = data;

       

        public NewsQueryServiceModel All(
            string searchTerm,
            int currentPage,
            int newsPerPage)
        {
            var newsQuery = this.data.News.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                newsQuery = newsQuery.Where(n =>
                    n.Title.ToLower().Contains(searchTerm.ToLower())
                    || n.Content.ToLower().Contains(searchTerm.ToLower()));
            }

            var news = newsQuery
                .OrderByDescending(n => n.Id)
                .Skip((currentPage - 1) * newsPerPage)
                .Take(newsPerPage)
                .Select(n => new NewsServiceModel()
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    ImageUrl = n.NewsImageUrl,
                    VideoUrl = n.NewsVideoUrl
                })
                .ToList();

            return new NewsQueryServiceModel
            {
                News = news
            };
        }

        public void Add(string title, string content, string imageUrl, string videoUrl)
        {
            var newsData = new News
            {
                Title = title,
                Content = content,
                NewsImageUrl = imageUrl,
                NewsVideoUrl = videoUrl ?? null
            };

            this.data.News.Add(newsData);

            this.data.SaveChanges();
        }

        public News GetNewsById(int id)
            => this.data
                .News
                .FirstOrDefault(x => x.Id == id);

        public void Delete(int id)
        {
            var news = this.GetNewsById(id);

            this.data.News.Remove(news);

            this.data.SaveChanges();
        }
    }
}
