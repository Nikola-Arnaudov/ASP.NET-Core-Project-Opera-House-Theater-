namespace OperaHouseTheater.Test.Routing
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using OperaHouseTheater.Areas.Admin.Controllers;
    using OperaHouseTheater.Models.Employee;

    public class EmployeeControllerTest
    {
        [Fact]
        public void GetAddRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Admin/Employee/Add")
                .To<EmployeeController>(c => c.Add());

        [Fact]
        public void PostAddRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Admin/Employee/Add")
                    .WithMethod(HttpMethod.Post))
                .To<EmployeeController>(c => c.Add(With.Any<AddEmployeeFormModel>()));

    }
}
