﻿namespace OperaHouseTheater.Services.Tickets
{

    public interface ITicketService
    {
        TicketQueryServiceModel All(string userId);

        bool Delete(int id);

    }
}
