namespace OperaHouseTheater.Test.Routing
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using OperaHouseTheater.Areas.Admin.Controllers;
    using OperaHouseTheater.Models.Performance;

    public class PerformanceControllerTest
    {
        [Fact]
        public void GetAddRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Admin/Performance/Create")
                .To<PerformanceController>(c => c.Create());

        [Fact]
        public void PostAddRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Admin/Performance/Create")
                    .WithMethod(HttpMethod.Post))
                .To<PerformanceController>(c => c.Create(With.Any<PerformanceFormModel>()));


    }
}
