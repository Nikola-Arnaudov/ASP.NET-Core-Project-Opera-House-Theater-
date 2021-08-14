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

        bool Return(int id);

        bool Delete(int id);

        bool IsCurrMembersTicket(int ticketId, int memberId);

        bool PerformanceTicketsExists(int performanceId);

        void CleareExpiredTickets(int performanceId);

        bool TicketExist(int id);
    }
}
