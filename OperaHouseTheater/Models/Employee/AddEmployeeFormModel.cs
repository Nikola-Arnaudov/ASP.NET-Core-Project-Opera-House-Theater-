namespace OperaHouseTheater.Models.Employee
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using static Data.DataConstants.Employee;

    public class AddEmployeeFormModel
    {
        [Required(ErrorMessage = "This field is required")]
        [StringLength(NameMaxLength,
            MinimumLength = NameMinLength,
            ErrorMessage = "Name must be between {2} & {1} symbols.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(NameMaxLength,
            MinimumLength = NameMinLength,
            ErrorMessage = "Last name must be between {2} & {1} symbols.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Url]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(BiographyMaxLength,
            MinimumLength = BiographyMinLength,
            ErrorMessage = "Biography must be between {2} & {1} symbols.")]
        public string Biography { get; set; }

        public int DepartmentId { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<EmployeeCategoryViewModel> EmployeeCategories { get; set; }

        public IEnumerable<EmployeeDepartmentViewModel> EmployeeDepartments { get; set; }
    }
}
