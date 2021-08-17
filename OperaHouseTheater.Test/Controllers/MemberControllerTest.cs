namespace OperaHouseTheater.Test.Controllers
{
    using System.Linq;
    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using OperaHouseTheater.Controllers.Member;
    using OperaHouseTheater.Models.Member;
    using OperaHouseTheater.Data.Models;
    using OperaHouseTheater.Controllers;

    public class MemberControllerTest
    {
        [Fact]
        public void BecomeShouldBeForAuthorizedUsersAndReturnView()
            => MyController<MemberController>
                .Instance()
                .Calling(c => c.Become())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Theory]
        [InlineData("Name", "0888888888")]
        public void PostBecomeShouldBeForAuthorizedUsersAndReturnRedirectWithValidModel(
            string memberName,
            string phoneNumber)
            => MyController<MemberController>
                .Instance(controller => controller
                    .WithUser(user => user
                        .WithIdentifier(TestUser.Identifier)))
                .Calling(c => c.Become(new BecomeMemberFormModel
                {
                    MemberName = memberName,
                    PhoneNumber = phoneNumber
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .ValidModelState()
                .Data(data => data.
                    WithSet<Member>(members => members
                        .Any(m =>
                            m.MemberName == memberName && 
                            m.PhoneNumber == phoneNumber &&
                            m.UserId == TestUser.Identifier)))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<HomeController>(c=> c.Index()));
    }
}
