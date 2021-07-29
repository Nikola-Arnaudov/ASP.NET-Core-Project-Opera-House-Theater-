namespace OperaHouseTheater.Models.Home
{
    using OperaHouseTheater.Models.Event;
    using OperaHouseTheater.Models.News;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


    public class HomeViewModel
    {
        public List<EventsListingViewModel> Events { get; set; }

        public List<NewsListingViewModel> News { get; set; }
    }
}
