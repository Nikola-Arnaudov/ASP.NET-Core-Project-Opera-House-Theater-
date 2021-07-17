namespace OperaHouseTheater.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class News
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NewsTitleMaxLength)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string NewsPictureUrl { get; set; }

        public string NewsVideoUrl { get; set; }
    }
}
