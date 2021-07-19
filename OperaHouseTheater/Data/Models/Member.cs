namespace OperaHouseTheater.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static DataConstants;

    public class Member
    {
        public int Id { get; init; }

        [Required]
        [StringLength(MemberNameMaxLength)]
        public string MemberName { get; set; }

        [Required]
        public string UserId { get; set; }

        public IEnumerable<Comment> Comments { get; init; } = new List<Comment>();

        public IEnumerable<Ticket> Tickets { get; init; } = new List<Ticket>();
    }
}
