namespace OperaHouseTheater.Test.Routing
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using OperaHouseTheater.Areas.Admin.Controllers;
    using OperaHouseTheater.Models.Event;

    public class EventControllerTest
    {
        [Fact]
        public void GetCreateRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Admin/Event/Create")
                .To<EventController>(c => c.Create());

        [Fact]
        public void PostCreateRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Admin/Event/Create")
                    .WithMethod(HttpMethod.Post))
                .To<EventController>(c => c.Create(With.Any<CreateEventFormModel>()));


        [Fact]
        public void PostSetRoleRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Admin/Event/SetRole")
                    .WithMethod(HttpMethod.Post))
                .To<EventController>(c => c.SetRole(With.Any<SetEventRoleFormModel>()));
    }
}
