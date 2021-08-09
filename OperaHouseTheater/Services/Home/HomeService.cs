namespace OperaHouseTheater.Services.Home
{
    using OperaHouseTheater.Data;
    using OperaHouseTheater.Services.Home.Models;
    using OperaHouseTheater.Services.News;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class HomeService : IHomeService
    {
        private readonly OperaHouseTheaterDbContext data;

        public HomeService(OperaHouseTheaterDbContext data) 
            => this.data = data;

        public HomeServiceModel IndexAll()
        {
            var indexPageData = new HomeServiceModel()
            {
                Events = this.data
                        .Events
                        .OrderBy(x => x.Date)
                        .Select(e => new EventsHomeServiceModel
                        {
                            Id = e.Id,
                            EventName = e.Performance.Title,
                            Composer = e.Performance.Composer,
                            Date = e.Date,
                            PerformanceType = e.Performance.PerformanceType.Type,
                            ImageUrl = e.Performance.ImageUrl
                        })
                        .Take(3)
                        .ToList(),
                News = this.data
                        .News
                        .OrderByDescending(x => x.Id)
                        .Select(n => new NewsServiceModel
                        {
                            Id = n.Id,
                            Title = n.Title,
                            Content = n.Content,
                            ImageUrl = n.NewsImageUrl,
                            VideoUrl = n.NewsVideoUrl,
                        })
                        .Take(3)
                        .ToList()
            };

            return indexPageData;
        }
    }
}
