namespace OperaHouseTheater.Models.Employee
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using static Data.DataConstants.Employee;

    public class AddEmployeeFormModel : IValidatableObject
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

        [Display(Name ="Department")]
        public int DepartmentId { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<EmployeeCategoryViewModel> EmployeeCategories { get; set; }

        public IEnumerable<EmployeeDepartmentViewModel> EmployeeDepartments { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var property = new[] { "DepartmentId" };

            if (this.DepartmentId == 1 && this.CategoryId < 6 || this.CategoryId > 10 ||
                this.DepartmentId == 2 && this.CategoryId < 1 || this.CategoryId > 5 ||
                this.DepartmentId == 3 && this.CategoryId < 11 || this.CategoryId > 15)
            {
                yield return new ValidationResult("Department is not responsible for this category.",property);
            }
         }
    }
}
