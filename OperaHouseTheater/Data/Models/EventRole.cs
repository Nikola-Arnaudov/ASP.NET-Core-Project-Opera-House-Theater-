namespace OperaHouseTheater.Data.Models
{
    public class EventRole
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public  Event Event { get; set; }

        public int RoleId { get; set; }

        public Role Role { get; set; }

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }
    }
}
