namespace OperaHouseTheater.Test.Controllers
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using OperaHouseTheater.Areas.Admin.Controllers;

    public class NewsControllerTest
    {
        [Fact]
        public void GetAddShouldBeForAuthorizedUsers()
            => MyController<NewsController>
                .Instance()
                .Calling(c => c.Add())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests());

        [Fact]
        public void DeleteShouldBeForAuthorizedUsers()
            => MyController<NewsController>
                .Instance()
                .Calling(c => c.Delete(1))
                .ShouldHave()
                .ActionAttributes(a => a.RestrictingForAuthorizedRequests())
                .AndAlso();

    }
}
