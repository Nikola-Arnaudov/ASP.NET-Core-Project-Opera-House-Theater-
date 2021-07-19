namespace OperaHouseTheater.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class Comment
    {
        public int Id { get; init; }

        public int MemberId { get; init; }

        public Member Member { get; set; }

        public int PerformanceId { get; set; }

        public Performance Performance { get; set; }

        [Required]
        [StringLength(CommentMaxLength)]
        public string Content { get; set; }
    }
}
