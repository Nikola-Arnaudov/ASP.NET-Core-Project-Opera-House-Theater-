namespace OperaHouseTheater.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Performance;

    public class Performance
    {
        public int Id { get; set; }

        [Required]
        [StringLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(ComposerNameMaxLength)]
        public string Composer { get; set; }

        [Required]
        public string Synopsis { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public int PerformanceTypeId { get; set; }

        public PerformanceType PerformanceType { get; set; }

        public IEnumerable<Role> Roles { get; set; } = new List<Role>();

        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();

        public IEnumerable<Event> Events { get; set; } = new List<Event>();

    }
}
