namespace OperaHouseTheater.Models.Home
{
    using OperaHouseTheater.Models.Event;
    using OperaHouseTheater.Models.News;
    using OperaHouseTheater.Services.News;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


    public class HomeViewModel
    {
        public List<EventsListingViewModel> Events { get; set; }

        public List<NewsServiceModel> News { get; set; }
    }
}
