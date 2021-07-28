namespace OperaHouseTheater.Models.Event
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class SetEventRoleFormModel
    {
        [Display(Name = "Заглавие")]
        [Required(ErrorMessage = "This field is required.")]
        public int RoleId { get; set; }

        public int PerformanceId { get; set; }

        public int EventId { get; set; }

        public int EmployeeId { get; set; }

        public IEnumerable<EventRoleModel> Roles { get; set; }

        public IEnumerable<EventEmployeeModel> Employees { get; set; }
    }
}
