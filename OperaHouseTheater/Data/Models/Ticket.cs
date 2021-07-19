namespace OperaHouseTheater.Data.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        public int MemberId { get; init; }

        public Member Member { get; set; }

        public int EventId { get; set; }

        public Event Event { get; set; }

        public int Count { get; set; }
    }
}
