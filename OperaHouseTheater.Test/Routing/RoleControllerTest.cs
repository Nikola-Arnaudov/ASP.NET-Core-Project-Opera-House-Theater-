namespace OperaHouseTheater.Test.Routing
{
    using MyTested.AspNetCore.Mvc;
    using OperaHouseTheater.Areas.Admin.Controllers;
    using OperaHouseTheater.Models.Performance;
    using Xunit;

    public class RoleControllerTest
    {
        [Fact]
        public void GetAddRoleRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Admin/Role/AddRole")
                .To<RoleController>(c => c.AddRole());

        [Fact]
        public void PostAddRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Admin/Role/AddRole")
                    .WithMethod(HttpMethod.Post))
                .To<RoleController>(c => c.AddRole(With.Any<AddRoleFormModel>()));

    }
}
