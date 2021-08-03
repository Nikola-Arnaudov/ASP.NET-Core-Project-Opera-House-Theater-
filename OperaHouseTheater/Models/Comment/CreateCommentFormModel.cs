namespace OperaHouseTheater.Models.Comment
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;

    public class CreateCommentFormModel
    {
        public int MemberId { get; set; }

        public int PerformanceId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(CommentMaxLength,
            MinimumLength = CommentMinLength,
            ErrorMessage = "Text must be between {2} & {1} symbols.")]
        public string Text { get; set; }

    }
}
