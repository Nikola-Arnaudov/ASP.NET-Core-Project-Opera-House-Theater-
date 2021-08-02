namespace OperaHouseTheater.Models.Ticket
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class BuyTicketFormModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Composer { get; set; }

        [Required]
        public string PerformanceType { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public DateTime Date { get; set; }

        public int FreeSeats { get; set; }

        public int TicketPrice { get; set; }

        [Required]
        public int CurrEventId { get; set; }

        public int SeatsCount { get; set; }

    }
}
