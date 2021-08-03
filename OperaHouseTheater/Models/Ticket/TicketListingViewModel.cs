namespace OperaHouseTheater.Models.Ticket
{
    using System;

    public class TicketListingViewModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Title { get; set; }

        public int SeatsCount { get; set; }

        public int Amount { get; set; }

        public int EventId { get; set; }
    }
}
