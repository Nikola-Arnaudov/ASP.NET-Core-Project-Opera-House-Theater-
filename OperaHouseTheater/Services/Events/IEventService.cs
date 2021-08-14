namespace OperaHouseTheater.Services.Events
{
    using OperaHouseTheater.Services.Events.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IEventService
    {
        EventQueryServiceModel All(string searchTerm, string type, int currentPage, int eventsPerPage);

        EventDetailsServiceModel Details(int id);

        int Create(int performanceId, DateTime datetime, int ticketPrice);

        bool Delete(int id);

        void SetRole(int roleId,int employeeId,int eventId);

        IEnumerable<EventEmployeeServiceModel> GetEmployees();

        IEnumerable<EventRoleServiceDropdownModel> GetRolesNames(int id);

        IEnumerable<PerformanceTitleServiceModel> GetPerformanceTitles();

        EventTicketServiceModel GetEventById(int id);

        int DeleteEventRole(int id);

        bool RolesPerformanceExist(int id);

        bool EventExist(int id);

        bool EventTicketsExist(int id);

        bool EventIsOver(int id);
    }
}
