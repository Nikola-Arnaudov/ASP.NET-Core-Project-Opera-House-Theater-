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

        public int FreeSeats { get; set; } = 500;

        public int TicketPrice { get; set; }

        public IEnumerable<EventRole> EventRoles { get; set; } = new List<EventRole>();

        public IEnumerable<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
