﻿namespace OperaHouseTheater.Models.Event
{
    using OperaHouseTheater.Services.Events;
    using OperaHouseTheater.Services.Events.Models;
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

        public IEnumerable<EventRoleServiceDropdownModel> Roles { get; set; }

        public IEnumerable<EventEmployeeServiceModel> Employees { get; set; }
    }
}
