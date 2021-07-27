namespace OperaHouseTheater.Models.Performance
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;

    public class AddRoleFormModel
    {
        [Required(ErrorMessage = "This field is required")]
        [StringLength(RoleNameMaxLength,
            MinimumLength = RoleNameMinLength,
            ErrorMessage = "Role name must be between {2} & {1} symbols.")]
        public string Name { get; set; }

        public int PerformanceId { get; set; }

        public IEnumerable<PerformanceTitleViewModel> PerformanceTitles { get; set; }
    }
}
