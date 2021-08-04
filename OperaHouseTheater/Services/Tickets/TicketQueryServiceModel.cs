namespace OperaHouseTheater.Services.Tickets
{

    using System.Collections.Generic;

    public class TicketQueryServiceModel
    {
        public int Id { get; set; }

        public string MemberName { get; set; }

        public IEnumerable<TicketServiceModel> Tickets { get; set; }
    }
}
