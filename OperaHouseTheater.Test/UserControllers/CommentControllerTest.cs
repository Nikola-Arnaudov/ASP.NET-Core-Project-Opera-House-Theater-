namespace OperaHouseTheater.Test.UserControllers
{
    using MyTested.AspNetCore.Mvc;
    using OperaHouseTheater.Controllers;
    using OperaHouseTheater.Models.Comment;
    using Xunit;

    public class CommentControllerTest
    {
        [Fact]
        public void GetCreateRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Comment/Create/1")
                .To<CommentController>(c => c.Create(1));

        [Fact]
        public void PostCreateRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Comment/Create")
                    .WithMethod(HttpMethod.Post))
                .To<CommentController>(c => c.Create(With.Any<CreateCommentFormModel>()));

    }
}
