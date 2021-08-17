namespace OperaHouseTheater.Controllers.Event
{
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Models.Event;
    using OperaHouseTheater.Services.Events;
    
    using static WebConstants;

    public class EventController : Controller
    {
        private readonly IEventService events;

        public EventController(IEventService events) 
            => this.events = events;

        public IActionResult All([FromQuery]AllEventsQueryModel query)
        {
            var queryResult = this.events.All(
                query.SearchTerm,
                query.Type, 
                query.CurrentPage,
                AllEventsQueryModel.EventsPerPage);

            query.Types = queryResult.Types;
            query.Events = queryResult.Events;
            query.EventsCount = queryResult.EventsCount;

            return View(query);
        }

        public IActionResult Details(int id) 
        {
            var eventData = this.events.Details(id);

            if (eventData == null)
            {
                TempData["ErrorMessage"] = "Invalid event.";

                return RedirectToAction("Error", "Home");
            }

            return View(eventData);
        }

    }
}
