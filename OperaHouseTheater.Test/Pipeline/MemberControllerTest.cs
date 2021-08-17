namespace OperaHouseTheater.Test.Pipeline
{
    using MyTested.AspNetCore.Mvc;
    using OperaHouseTheater.Controllers;
    using OperaHouseTheater.Controllers.Member;
    using OperaHouseTheater.Data.Models;
    using OperaHouseTheater.Models.Member;
    using System.Linq;
    using Xunit;

    public class MemberControllerTest
    {

        

        [Fact]
        public void GetBecomeShouldBeForAuthorizredUsersAndReturnView()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Member/Become")
                    .WithUser())
                .To<MemberController>(c => c.Become())
                .Which()
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
             => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Member/Become")
                    .WithMethod(HttpMethod.Post)
                    .WithFormFields(new 
                    { 
                        MemberName = memberName,
                        PhoneNumber = phoneNumber
                    })
                    .WithUser())
                 .To<MemberController>(c => c.Become(new BecomeMemberFormModel
                 {
                     MemberName = memberName,
                     PhoneNumber = phoneNumber
                 }))
                .Which()
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
                     .To<HomeController>(c => c.Index()));

        [Fact]
        public void BecomeShouldBeForAuthorizredUserAndShouldReturnView()
            =>  MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithPath("/Member/Become")
                    .WithUser())
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
