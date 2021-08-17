namespace OperaHouseTheater.Test.Controllers
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using OperaHouseTheater.Areas.Admin.Controllers;

    public class EventControllerTest
    {
        [Fact]
        public void GetCreateShouldBeForAuthorizedUsers()
            => MyController<EventController>
                .Instance()
                .Calling(c => c.Create())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests());

        [Fact]
        public void DeleteShouldBeForAuthorizedUsers()
            => MyController<EventController>
                .Instance()
                .Calling(c => c.Delete(1))
                .ShouldHave()
                .ActionAttributes(a => a.RestrictingForAuthorizedRequests())
                .AndAlso();

        [Fact]
        public void DeleteEventRoleShouldBeForAuthorizedUsers()
            => MyController<EventController>
                .Instance()
                .Calling(c => c.DeleteEventRole(1))
                .ShouldHave()
                .ActionAttributes(a => a.RestrictingForAuthorizedRequests())
                .AndAlso();
    }
}
