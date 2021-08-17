namespace OperaHouseTheater.Services.Events
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using OperaHouseTheater.Data;
    using OperaHouseTheater.Data.Models;
    using OperaHouseTheater.Services.Events.Models;

    using static Data.DataConstants;

    public class EventService : IEventService
    {
        private readonly OperaHouseTheaterDbContext data;

        public EventService(OperaHouseTheaterDbContext data) 
            => this.data = data;

        public EventQueryServiceModel All(string searchTerm,string type,int currentPage,int eventsPerPage)
        {
            var eventsQuery = this.data.Events.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                eventsQuery = eventsQuery.Where(e =>
                    e.Performance.Title.ToLower().Contains(searchTerm.ToLower())
                    || e.Performance.Composer.ToLower().Contains(searchTerm.ToLower())
                    || e.Performance.PerformanceType.Type.ToLower().Contains(searchTerm.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(type))
            {
                eventsQuery = eventsQuery.Where(e => e.Performance.PerformanceType.Type == type);
            }

            var events = eventsQuery
                .OrderBy(e => e.Date)
                .Skip((currentPage - 1) * eventsPerPage)
                .Take(eventsPerPage)
                .Select(e => new EventListingServiceModel()
                {
                    Id = e.Id,
                    EventName = e.Performance.Title,
                    Composer = e.Performance.Composer,
                    PerformanceType = e.Performance.PerformanceType.Type,
                    Date = e.Date,
                    ImageUrl = e.Performance.ImageUrl
                })
                .ToList();

            var types = this.data
                .PerformanceTypes
                .Select(t => t.Type)
                .ToList();

            return new EventQueryServiceModel
            {
                Types = types,
                Events = events,
                EventsCount = eventsQuery.Count()
            };
        }

        public EventDetailsServiceModel Details(int id)
        {
            var crrEvent = this.data
               .Events
               .FirstOrDefault(x => x.Id == id);

            if (crrEvent == null)
            {
                return null;
            }

            var crrPerformance = this.data.Performances.FirstOrDefault(x => x.Id == crrEvent.PerformanceId);

            var eventData = new EventDetailsServiceModel
            {
                Id = crrEvent.Id,
                Title = crrPerformance.Title,
                Composer = crrPerformance.Composer,
                Synopsis = crrPerformance.Synopsis,
                ImageUrl = crrPerformance.ImageUrl,
                Date = crrEvent.Date,
                PerformanceId = crrPerformance.Id,
                EventRoles = data.EventRoles.Where(e => e.EventId == crrEvent.Id)
                        .Select(r => new EventRoleServiceModel
                        {
                            Id = r.Id,
                            RoleName = r.Role.RoleName,
                            EmployeeFirstName = r.Employee.FirstName,
                            EmployeeLastName = r.Employee.LastName,
                            ImageUrl = r.Employee.ImageUrl,
                            EmployeeId = r.EmployeeId,
                            EventId = r.EventId
                        }).ToList(),
            };

            return eventData;
        }

        public int Create(int performanceId, DateTime datetime, int ticketPrice)    
        {
            var eventData = new Event
            {
                PerformanceId = performanceId,
                Date = datetime,
                TicketPrice = ticketPrice,
                Performance = data.Performances.FirstOrDefault(x => x.Id == performanceId),
                FreeSeats = DefaultFreeSeats
            };

            this.data.Events.Add(eventData);
            this.data.SaveChanges();

            return eventData.Id;
        }

        public bool Delete(int id)
        {
            var crrEvent = this.data.Events
                .FirstOrDefault(x => x.Id == id);

            var eventRoles = this.data
                .EventRoles
                .Where(er => er.EventId == id)
                .ToList();

            this.data.EventRoles.RemoveRange(eventRoles);
            this.data.SaveChanges();

            var ticketsForTheEvent = this.data
                .Tickets
                .Where(t => t.EventId == id)
                .ToList();

            this.data.Tickets.RemoveRange(ticketsForTheEvent);
            this.data.SaveChanges();

            this.data.Events.Remove(crrEvent);
            this.data.SaveChanges();

            return true;
        }

        public void SetRole(int roleId, int employeeId, int eventId)
        {
            var roleData = new EventRole
            {
                RoleId = roleId,
                EmployeeId = employeeId,
                EventId = eventId,
            };

            this.data.EventRoles.Add(roleData);
            this.data.SaveChanges();
        }

        public int DeleteEventRole(int id)
        {
            var crrEventRole = this.data.EventRoles
                .FirstOrDefault(e => e.Id == id);

            if (crrEventRole == null)
            {
                return 0;
            }

            this.data.EventRoles.Remove(crrEventRole);
            this.data.SaveChanges();

            return crrEventRole.EventId;
        }

        public IEnumerable<EventEmployeeServiceModel> GetEmployees()
            => this.data.Employees
                .Select(e => new EventEmployeeServiceModel
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Category = e.Category.CategoryName
                });

        public IEnumerable<EventRoleServiceDropdownModel> GetRolesNames(int id)
            => this.data.RolesPerformance
                .Where(r => r.PerformanceId == id)
                .Select(r => new EventRoleServiceDropdownModel
                {
                    Id = r.Id,
                    RoleName = r.RoleName,
                });

        public IEnumerable<PerformanceTitleServiceModel> GetPerformanceTitles()
           => this.data
               .Performances
               .Select(p => new PerformanceTitleServiceModel
               {
                   Id = p.Id,
                   Title = p.Title
               })
               .ToList();

        public bool RolesPerformanceExist(int id)
            => this.data.RolesPerformance.Any(r => r.Id == id);

        public EventTicketServiceModel GetEventById(int id)
            => this.data.Events
            .Where(x => x.Id == id)
            .Select(x=>new EventTicketServiceModel
            {
                Id= x.Id,
                FreeSeats = x.FreeSeats,
                TicketPrice = x.TicketPrice,
                Date = x.Date,
                PerformanceId = x.PerformanceId
            })
            .FirstOrDefault();

        public bool EventExist(int id)
            => this.data
            .Events
            .Any(e => e.Id == id);

        public bool EventTicketsExist(int id)
            => this.data
            .Tickets
            .Any(t => t.EventId == id);

        public bool EventIsOver(int id)
            => this.data.Events
            .Where(e => e.Id == id && e.Date > DateTime.UtcNow)
            .Any();
    }
}
