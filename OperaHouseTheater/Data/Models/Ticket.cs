namespace OperaHouseTheater.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Performance;

    public class Ticket
    {
        public int Id { get; init; }

        public int MemberId { get; init; }

        public Member Member { get; init; }

        public int EventId { get; init; }

        public Event Event { get; init; }

        public int SeatsCount { get; init; }

        public int Amount { get; init; }

        public DateTime Date { get; init; }

        [Required]
        [StringLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(ComposerNameMaxLength)]
        public string Composer { get; set; }

        [Required]
        public string PerformanceType { get; set; }
    }
}
