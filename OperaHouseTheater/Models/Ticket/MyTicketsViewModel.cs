namespace OperaHouseTheater.Models.Ticket
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


    public class MyTicketsViewModel
    {
        public int Id { get; set; }

        public string MemberName { get; set; }

        public IEnumerable<TicketListingViewModel> Tickets { get; set; }
    }
}
