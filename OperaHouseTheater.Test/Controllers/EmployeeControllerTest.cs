namespace OperaHouseTheater.Test.Controllers
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using OperaHouseTheater.Areas.Admin.Controllers;

    public class EmployeeControllerTest
    {
        [Fact]
        public void AddShouldBeForAuthorizedUsers()
            => MyController<EmployeeController>
                .Instance()
                .Calling(c => c.Add())
                .ShouldHave()
                .ActionAttributes(a => a.RestrictingForAuthorizedRequests());

        [Fact]
        public void DeleteShouldBeForAuthorizedUsers()
            => MyController<EmployeeController>
                .Instance()
                .Calling(c => c.Delete(1))
                .ShouldHave()
                .ActionAttributes(a => a.RestrictingForAuthorizedRequests())
                .AndAlso();
    }
}
