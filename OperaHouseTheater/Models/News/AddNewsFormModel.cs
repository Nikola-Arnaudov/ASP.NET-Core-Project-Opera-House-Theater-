namespace OperaHouseTheater.Models.News
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.News;

    public class AddNewsFormModel
    {
        [Required]
        [StringLength(NewsTitleMaxLength, 
            MinimumLength = NewsTitleMinLength,
            ErrorMessage = "Title length must be between {2} & {1} symbols.")]
        public string Title { get; set; }

        [Required]
        [MinLength(NewsContentMinLength,
            ErrorMessage = "Тhe text must be at least {1} symbols long.")]
        public string Content { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }

        [Url]
        public string VideoUrl { get; set; }
    }
}
