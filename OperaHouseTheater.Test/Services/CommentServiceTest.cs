namespace OperaHouseTheater.Test.Services
{
    using Xunit;
    using OperaHouseTheater.Test.Mocks;


    using OperaHouseTheater.Services.Comments;
    using Moq;
    using OperaHouseTheater.Data.Models;
    using System.Linq;

    public class CommentServiceTest
    {
        [Fact]
        public void CurrentPerformanceExistShouldReturnIdIfThatExists()
        {
            var id = 1;

            var data = DatabaseMock.Instance;

            data.Performances.Add(new Performance { });
            data.SaveChanges();

            var commentService = new Mock<CommentService>(data);
            var result = commentService.Object.CurrentPerformanceExist(id);

            Assert.True(result);
        }

        [Fact]
        public void CurrentPerformanceExistShouldReturnZeroIfNotExist()
        {
            var id = 100;

            var data = DatabaseMock.Instance;

            data.Performances.Add(new Performance { });
            data.SaveChanges();

            var commentService = new Mock<CommentService>(data);
            var result = commentService.Object.CurrentPerformanceExist(id);

            Assert.False(result);
        }

        [Fact]
        public void DeleteShouldActCorrect() 
        {
            var id = 1;

            var data = DatabaseMock.Instance;

            data.Comments.Add(new Comment { });
            data.SaveChanges();

            var commentService = new Mock<CommentService>(data);
            commentService.Object.Delete(id);

            var result = data.Comments.Any(x => x.Id == id);

            Assert.True(result == false);
        }

        [Fact]
        public void CreateShouldActCorrectly()
        {
            var id = 1;

            var data = DatabaseMock.Instance;

            var commentService = new Mock<CommentService>(data);
            commentService.Object.Create(1,2,"ok");

            var result = data.Comments.Any(x => x.Id == id);

            Assert.True(result);
        }

    }
}
