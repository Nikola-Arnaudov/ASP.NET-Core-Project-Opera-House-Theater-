namespace OperaHouseTheater.Services.Tickets
{
    using OperaHouseTheater.Data;
    using OperaHouseTheater.Data.Models;
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

        public void Buy(string userId,
            int ticketPrice,
            int seatsCount,
            string composer,
            string title,
            DateTime date,
            string performanceType,
            int currEventId)
        {
            
            var member = this.data
                .Members
                .FirstOrDefault(x => x.UserId == userId);

            var ticketData = new Ticket
            {
                Amount = ticketPrice * seatsCount,
                Composer = composer,
                Title = title,
                SeatsCount = seatsCount,
                Date = date,
                PerformanceType = performanceType,
                EventId = currEventId,
                MemberId = member.Id,
            };

            var crrEvent = this.data.Events.FirstOrDefault(x => x.Id == currEventId);
            crrEvent.FreeSeats -= seatsCount;
            data.SaveChanges();

            this.data.Tickets.Add(ticketData);
            this.data.SaveChanges();
        }

        public bool Delete(int id)
        {
            var ticket = this.data.Tickets.FirstOrDefault(t => t.Id == id);

            if (ticket == null)
            {
                return false;
            }
            else
            {
                this.data.Tickets.Remove(ticket);
                this.data.SaveChanges();
            }

            return true;
        }

        public bool IsCurrMembersTicket(int ticketId, int memberId)
            => this.data.Tickets.Any(t => t.Id == ticketId && t.MemberId == memberId);

        public bool TicketsExists(int performanceId)
        {
            var events = this.data.Events.Where(e => e.PerformanceId == performanceId);

            foreach (var crrEvent in events)
            {
                var tickets = this.data
                    .Tickets
                    .Where(t => t.EventId == crrEvent.Id);

                if (tickets.Any(t => t.Date > DateTime.UtcNow))
                {
                    return true;
                }
            }

            foreach (var crrEvent in events)
            {
                var tickets = this.data
                    .Tickets
                    .Where(t => t.EventId == crrEvent.Id);

                this.data.RemoveRange(tickets);
            }

            this.data.SaveChanges();

            return false;
        }

        public void CleareExpiredTickets(int performanceId) 
        {
            var events = this.data.Events.Where(e => e.PerformanceId == performanceId);

            foreach (var crrEvent in events)
            {
                var tickets = this.data
                    .Tickets
                    .Where(t => t.EventId == crrEvent.Id);

                this.data.RemoveRange(tickets);
            }

            this.data.SaveChanges();
        }

        
    }
}
