namespace OperaHouseTheater.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Controllers.Member;
    using OperaHouseTheater.Data;
    using OperaHouseTheater.Data.Models;
    using OperaHouseTheater.Infrastructure;
    using OperaHouseTheater.Models.Comment;
    using System.Linq;


    public class CommentController : Controller
    {
        private readonly OperaHouseTheaterDbContext data;

        public CommentController(OperaHouseTheaterDbContext data) 
            => this.data = data;


        [Authorize]
        public IActionResult Create(int id) 
        {
            var memberId = GetMemberId();

            if (memberId == 0)
            {
                //TODO
                //this.TempData

                return RedirectToAction(nameof(MemberController.Become), "Member");
            }

            var crrPerformance = this.data.Performances.FirstOrDefault(p => p.Id == id);

            //TODO: Error message
            if (crrPerformance == null)
            {
                return BadRequest();
            }

            var commentData = new CreateCommentFormModel
            {
                MemberId = memberId,
                PerformanceId = crrPerformance.Id
            };

            return View(commentData);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CreateCommentFormModel comment) 
        {
            if (!this.UserIsMember())
            {
                //TODO
                //this.TempData

                return RedirectToAction(nameof(MemberController.Become), "Member");
            }

            if (!ModelState.IsValid)
            {
                return View(comment);
            }

            var commentData = new Comment
            {
                MemberId = comment.MemberId,
                PerformanceId = comment.PerformanceId,
                Content = comment.Text
            };

            this.data.Comments.Add(commentData);
            this.data.SaveChanges();

            return Redirect($"/Performance/Details/{comment.PerformanceId}");
        }

        [Authorize]
        public IActionResult Delete(int id) 
        {
            var memberId = GetMemberId();

            var comment = this.data.Comments.FirstOrDefault(c => c.Id == id);

            if (comment == null)
            {
                //TODO: Error message

                return BadRequest();
            }

            if (comment.MemberId != memberId)
            {
                //TODO: Error message

                return BadRequest();
            }

            this.data.Comments.Remove(comment);
            this.data.SaveChanges();

            return Redirect($"/Performance/Details/{comment.PerformanceId}");
        }

        private bool UserIsMember()
            => this.data
                .Members
                .Any(x => x.UserId == this.User.GetId());

        private int GetMemberId()
            => this.data
                .Members
                .Where(m => m.UserId == this.User.GetId())
                .Select(m => m.Id)
                .FirstOrDefault();
    }
}
