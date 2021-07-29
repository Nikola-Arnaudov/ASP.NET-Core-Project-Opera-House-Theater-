namespace OperaHouseTheater.Models.Event
{
    using System;

    public class EventsListingViewModel
    {
        public int Id { get; set; }

        public string EventName { get; set; }

        public DateTime Date { get; set; }

        public string PerformanceType { get; set; }

        public string Composer { get; set; }

        public string ImageUrl { get; set; }

    }
}
