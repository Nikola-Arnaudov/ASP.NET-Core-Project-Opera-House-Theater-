namespace OperaHouseTheater.Test.Routing
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using OperaHouseTheater.Controllers.Member;
    using OperaHouseTheater.Models.Member;

    public class MemberControllerTest
    {
        [Fact]
        public void GetBecomeRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Member/Become")
                .To<MemberController>(c => c.Become());

        [Fact]
        public void PostBecomeRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Member/Become")
                    .WithMethod(HttpMethod.Post))
                .To<MemberController>(c => c.Become(With.Any<BecomeMemberFormModel>()));

        [Fact]
        public void BecomeShouldBeForAuthorizredUsersAndReturnView()
            => MyMvc
                .Routing()
                .ShouldMap("/Member/Become")
                .To<MemberController>(c => c.Become())
                .Which()
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();
    }
}

