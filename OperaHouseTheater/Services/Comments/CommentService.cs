namespace OperaHouseTheater.Services.Comments
{
    using System.Linq;
    using OperaHouseTheater.Data;
    using OperaHouseTheater.Data.Models;

    public class CommentService : ICommentService
    {
        private readonly OperaHouseTheaterDbContext data;

        public CommentService(OperaHouseTheaterDbContext data) 
            => this.data = data;

        public bool Create(int memberId, int performanceId, string content)
        {
            var commentData = new Comment
            {
                MemberId = memberId,
                PerformanceId = performanceId,
                Content = content
            };

            this.data.Comments.Add(commentData);
            this.data.SaveChanges();

            return true;
        }

        public void Delete(int id) 
        {
            var comment = GetCommentById(id);

            this.data.Comments.Remove(comment);
            this.data.SaveChanges();
        }

        public bool CurrentPerformanceExist(int id)
            => this.data.Performances.Any(p => p.Id == id);

        public Comment GetCommentById(int id)
            => this.data.Comments.FirstOrDefault(c => c.Id == id);
    }
}
