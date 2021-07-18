namespace OperaHouseTheater.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public class Event
    {
        public int Id { get; set; }

        [Required]
        public int PerformanceId { get; set; }

        public Performance Performance { get; set;  }

        [Required]
        public DateTime Date { get; set; }

        public int FreeSeats { get; set; } = 500;

        public int SeatsTaken { get; set; }

        public ICollection<Role> Roles { get; set; }

    }
}
