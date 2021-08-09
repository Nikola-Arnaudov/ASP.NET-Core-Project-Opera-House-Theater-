namespace OperaHouseTheater.Services.Home.Models
{
    using OperaHouseTheater.Services.News;
    using System.Collections.Generic;

    public class HomeServiceModel
    {
        public List<EventsHomeServiceModel> Events { get; set; }

        public List<NewsServiceModel> News { get; set; }
    }
}
