namespace OperaHouseTheater.Services.Tickets
{
    using System;


    public interface ITicketService
    {
        TicketQueryServiceModel All(string userId);

        void Buy(string userId,
            int ticketPrice,
            int seatsCount,
            string composer,
            string title,
            DateTime date,
            string performanceType,
            int currEventId);

        bool Delete(int id);

        bool IsCurrMembersTicket(int ticketId, int memberId);

        bool TicketsExists(int performanceId);

        void CleareExpiredTickets(int performanceId);
    }
}
