namespace OperaHouseTheater.Test.Pipeline
{
    using OperaHouseTheater.Controllers;
    using OperaHouseTheater.Data.Models;
    using OperaHouseTheater.Services.Home.Models;
    using FluentAssertions;
    using System.Linq;
    using MyTested.AspNetCore.Mvc;

    using Xunit;

    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnViewWithCorrectModelAndData()
            =>  MyMvc
                .Pipeline()
                .ShouldMap("/")
                .To<HomeController>(c=>c.Index())
                .Which(controller => controller
                    .WithData(Enumerable.Range(0, 10).Select(e => new News())))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<HomeServiceModel>()
                    .Passing(m=> m.News.Should().HaveCount(3)));

        [Fact]
        public void ErrorShouldReturnView()
            => MyMvc
                .Pipeline()
                .ShouldMap("/Home/Error")
                .To<HomeController>(c => c.Error())
                .Which()
                .ShouldReturn()
                .View();

    }
}
