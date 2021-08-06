namespace OperaHouseTheater.Services.Events
{
    using System.Collections.Generic;

    public class EventQueryServiceModel
    {
        public int EventsCount { get; set; }

        public IEnumerable<string> Types { get; init; }

        public IEnumerable<EventServiceModel> Events { get; init; }
    }
}
