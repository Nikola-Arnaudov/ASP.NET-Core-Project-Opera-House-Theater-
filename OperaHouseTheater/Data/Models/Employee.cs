namespace OperaHouseTheater.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Employee;

    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string LastName { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }

        [Required]
        [StringLength(BiographyMaxLength)]
        public string Biography { get; set; }

        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        public int CategoryId { get; set; }

        public EmployeeCategory Category { get; set; }

        public IEnumerable<EventRole> EventRoles { get; set; } = new List<EventRole>();
    }
}
