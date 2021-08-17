namespace OperaHouseTheater.Test.Routing
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using OperaHouseTheater.Areas.Admin.Controllers;
    using OperaHouseTheater.Models.News;

    public class NewsControllerTest
    {
        [Fact]
        public void GetAddRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Admin/News/Add")
                .To<NewsController>(c => c.Add());

        [Fact]
        public void PostAddRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Admin/News/Add")
                    .WithMethod(HttpMethod.Post))
                .To<NewsController>(c => c.Add(With.Any<AddNewsFormModel>()));


    }
}
