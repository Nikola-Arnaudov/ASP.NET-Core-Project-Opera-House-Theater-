namespace OperaHouseTheater.Services.Tickets
{
    using OperaHouseTheater.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TicketService : ITicketService 
    {
        private readonly OperaHouseTheaterDbContext data;

        public TicketService(OperaHouseTheaterDbContext data) 
            => this.data = data;

        public TicketQueryServiceModel All(string userId)
        {
            var member = this.data
                .Members
                .FirstOrDefault(m => m.UserId == userId);

            if (member == null)
            {
                //TODO Error Message

                return null;
            }

            var myTicketsData = new TicketQueryServiceModel
            {
                Id = member.Id,
                MemberName = member.MemberName,
                Tickets = this.data.Tickets
                        .Where(t => t.MemberId == member.Id)
                        .OrderBy(t => t.Date)
                        .Select(t => new TicketServiceModel
                        {
                            SeatsCount = t.SeatsCount,
                            Amount = t.Amount,
                            Date = t.Date,
                            Title = t.Title,
                            Id = t.Id,
                            EventId = t.EventId
                        })
                        .ToList()
            };

            return myTicketsData;
        }
    }
}
