namespace OperaHouseTheater.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class PerformanceType
    {
        public int Id { get; set; }

        [Required]
        public string Type { get; set; }

        public IEnumerable<Performance> Performances { get; set; } = new List<Performance>();
    }
}
