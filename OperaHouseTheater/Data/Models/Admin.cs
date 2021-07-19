namespace OperaHouseTheater.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Admin;
    public class Admin
    {
        public int Id { get; init; }

        [Required]
        [StringLength(NameMaxLength)]
        public string AdminName { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
