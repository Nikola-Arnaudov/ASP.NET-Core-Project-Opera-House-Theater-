namespace OperaHouseTheater.Models.Home
{
    using System.Collections.Generic;
    using OperaHouseTheater.Models.Event;
    using OperaHouseTheater.Services.News;


    public class HomeViewModel
    {
        public List<EventsListingViewModel> Events { get; set; }

        public List<NewsServiceModel> News { get; set; }
    }
}
