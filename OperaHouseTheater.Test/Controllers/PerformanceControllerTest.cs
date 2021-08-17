namespace OperaHouseTheater.Test.Controllers
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using OperaHouseTheater.Areas.Admin.Controllers;


    public class PerformanceControllerTest
    {
        [Fact]
        public void GetCreateShouldBeForAuthorizedUsers()
            => MyController<PerformanceController>
                .Instance()
                .Calling(c => c.Create())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests());

        [Fact]
        public void GetEditShouldBeForAuthorizedUsers()
            => MyController<PerformanceController>
                .Instance()
                .Calling(c => c.Edit(1))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests());

        [Fact]
        public void DeleteShouldBeForAuthorizedUsers()
            => MyController<PerformanceController>
                .Instance()
                .Calling(c => c.Delete(1))
                .ShouldHave()
                .ActionAttributes(a => a.RestrictingForAuthorizedRequests())
                .AndAlso();


    }
}
