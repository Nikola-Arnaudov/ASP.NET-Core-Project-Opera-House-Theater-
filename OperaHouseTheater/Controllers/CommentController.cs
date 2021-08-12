namespace OperaHouseTheater.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Controllers.Member;
    using OperaHouseTheater.Infrastructure;
    using OperaHouseTheater.Models.Comment;
    using OperaHouseTheater.Services.Comments;
    using OperaHouseTheater.Services.Members;
    using static WebConstants;

    public class CommentController : Controller
    {
        private readonly ICommentService comments;
        private readonly IMemberService members;

        public CommentController(
            ICommentService comments, 
            IMemberService members)
        {
            this.comments = comments;
            this.members = members;
        }

        [Authorize]
        //only Admin and member
        public IActionResult Create(int id) 
        {
            var memberId = this.members.GetMemberId(this.User.GetId());

            if (memberId == 0 && !User.IsInRole(AdministratorRoleName))
            {
                //TODO
                //this.TempData

                return RedirectToAction(nameof(MemberController.Become), "Member");
            }

            var crrPerformance = this.comments.CurrentPerformanceExist(id);

            //TODO: Error message
            if (crrPerformance == 0)
            {
                return BadRequest();
            }

            var commentData = new CreateCommentFormModel
            {
                MemberId = memberId,
                PerformanceId = crrPerformance
            };

            return View(commentData);
        }

        [Authorize]
        [HttpPost]
        //only Admin and member
        public IActionResult Create(CreateCommentFormModel comment) 
        {
            var memberId = this.members.GetMemberId(this.User.GetId());

            if (memberId == 0)
            {
                //TODO
                //this.TempData

                return RedirectToAction(nameof(MemberController.Become), "Member");
            }

            if (!ModelState.IsValid)
            {
                return View(comment);
            }

            this.comments.Create(comment.MemberId, comment.PerformanceId, comment.Text);

            return Redirect($"/Performance/Details/{comment.PerformanceId}");
        }

        [Authorize]
        //only Admin and member
        public IActionResult Delete(int id) 
        {
            var memberId = this.members.GetMemberId(this.User.GetId());

            var comment = this.comments.GetCommentById(id);

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

            this.comments.Delete(id);

            return Redirect($"/Performance/Details/{comment.PerformanceId}");
        }

    }
}
