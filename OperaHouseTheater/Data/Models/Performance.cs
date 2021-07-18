namespace OperaHouseTheater.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class Performance
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Type { get; set; }

        public string Composer { get; set; }

        public string Synopsis { get; set; }

        public string ImageUrl { get; set; }

        public ICollection<Role> Roles { get; set; }

    }
}
