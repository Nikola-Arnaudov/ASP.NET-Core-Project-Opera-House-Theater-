namespace OperaHouseTheater.Models.Event
{
    using OperaHouseTheater.Data.Models;
    using System;
    using System.Collections.Generic;

    public class EventDetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Synopsis { get; set; }

        public string Composer { get; set; }

        public int PerformanceId { get; set; }

        public string ImageUrl { get; set;  }

        public DateTime Date { get; set; }
        
        public IEnumerable<EventRoleListinViewModel> EventRoles { get; set; }
    }
}
