namespace OperaHouseTheater.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    using static DataConstants;

    public class EmployeeCategory
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(CategoryMaxLength)]
        public string CategoryName { get; set; }
        
        public ICollection<Employee> Employees{ get; set; }
    }
}
