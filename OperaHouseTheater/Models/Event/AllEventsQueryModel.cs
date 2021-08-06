namespace OperaHouseTheater.Models.Event
{
    using OperaHouseTheater.Services.Events;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AllEventsQueryModel
    {
        public const int EventsPerPage = 2;

        public string Type { get; init; }


        [Display(Name = "Search")]
        public string SearchTerm { get; init; }

        public int CurrentPage { get; set; } = 1;

        public int EventsCount { get; set; }

        public IEnumerable<string> Types { get; set; }

        public IEnumerable<EventServiceModel> Events { get; set; }
    }
}
