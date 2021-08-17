namespace OperaHouseTheater.Test.Controllers
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using OperaHouseTheater.Areas.Admin.Controllers;

    public class RoleControllerTest
    {
        [Fact]
        public void GetAddRoleShouldBeForAuthorizedUsers()
            => MyController<RoleController>
                .Instance()
                .Calling(c => c.AddRole())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests());


        [Fact]
        public void DeleteRoleShouldBeForAuthorizedUsers()
            => MyController<RoleController>
                .Instance()
                .Calling(c => c.Delete(1))
                .ShouldHave()
                .ActionAttributes(a => a.RestrictingForAuthorizedRequests())
                .AndAlso();

    }
}
