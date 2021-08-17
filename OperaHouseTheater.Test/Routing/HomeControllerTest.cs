namespace OperaHouseTheater.Test.Routing
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using OperaHouseTheater.Controllers;

    public class HomeControllerTest
    {
        [Fact]
        public void IndexRouteShouldBeMapper()
            => MyRouting
                .Configuration()
                .ShouldMap("/")
                .To<HomeController>(c => c.Index());

        [Fact]
        public void ErrorRouteShouldBeMapper()
            => MyRouting
                .Configuration()
                .ShouldMap("/Home/Error")
                .To<HomeController>(c => c.Error());
    }
}
