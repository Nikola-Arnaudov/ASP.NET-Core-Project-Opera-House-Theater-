namespace OperaHouseTheater.Services.Events.Models
{
    using System;

    public class EventTicketServiceModel
    {
        public int Id { get; init; }

        public DateTime Date { get; set; }

        public int PerformanceId { get; init; }

        public int FreeSeats { get; set; } = 500;

        public int TicketPrice { get; set; }

    }
}
