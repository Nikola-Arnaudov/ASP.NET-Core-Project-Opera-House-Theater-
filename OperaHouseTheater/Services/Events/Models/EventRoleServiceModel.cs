namespace OperaHouseTheater.Services.Events
{
    public class EventRoleServiceModel
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeFirstName { get; set; }

        public string EmployeeLastName { get; set; }

        public string ImageUrl { get; set; }

        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public int EventId { get; set; }
    }
}
