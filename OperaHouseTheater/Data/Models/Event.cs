namespace OperaHouseTheater.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Event
    {
        public int Id { get; init; }

        [Required]
        public int PerformanceId { get; init; }

        public Performance Performance { get; init;  }

        public DateTime Date { get; set; }

        public int FreeSeats { get; } = 500;

        public int SeatsTaken { get; } = 0;

        public int TicketPrice { get; set; }

        public IEnumerable<EventRole> EventRoles { get; set; } = new List<EventRole>();
    }
}
