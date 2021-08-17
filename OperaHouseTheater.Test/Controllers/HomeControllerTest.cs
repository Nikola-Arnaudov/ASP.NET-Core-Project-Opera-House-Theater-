namespace OperaHouseTheater.Test.Controllers
{
    using System.Linq;
    using Xunit;
    using FluentAssertions;
    using MyTested.AspNetCore.Mvc;
    using OperaHouseTheater.Data.Models;
    using OperaHouseTheater.Controllers;
    using OperaHouseTheater.Services.Home.Models;

    using static WebConstants.Cache;

    public class HomeControllerTest
    {
        [Fact]
        public void IndexActionShouldReturnCorrectViewWithModel()
            => MyController<HomeController>
                .Instance(instance => instance
                    .WithData(Enumerable.Range(0, 10).Select(e => new News())))
                .Calling(c => c.Index())
                .ShouldHave()
                .MemoryCache(cache=> cache
                    .ContainingEntryWithKey(IndexPageCacheKey))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<HomeServiceModel>()
                    .Passing(model => model.News.Should().HaveCount(3)));

        [Fact]
        public void ErrorShouldReturnView()
            => MyController<HomeController>
                .Instance()
                .Calling(c => c.Error())
                .ShouldReturn()
                .View();
                
    }
}
