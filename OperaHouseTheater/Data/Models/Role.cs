namespace OperaHouseTheater.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class Role
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(RoleNameMaxLength)]
        public string RoleName { get; set; }

        [Required]
        [MaxLength(RoleNameMaxLength)]
        public string ArtistName { get; set; }

        public int PerformanceId { get; set; }

        public Performance Performance { get; set; }

        public string UserId { get; set; }
    }
}
