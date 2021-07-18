namespace OperaHouseTheater.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    using static DataConstants;

    public class EmployeeCategory
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(EmployeeCategoryMaxLength)]
        public string CategoryName { get; set; }

        public int DepartmentId { get; set; }

        public Department Department { get; set; }
        
        public ICollection<Employee> Employees{ get; set; }
    }
}
