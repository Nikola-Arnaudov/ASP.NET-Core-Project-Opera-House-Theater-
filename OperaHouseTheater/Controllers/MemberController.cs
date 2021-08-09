namespace OperaHouseTheater.Controllers.Member
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Infrastructure;
    using OperaHouseTheater.Models.Member;
    using OperaHouseTheater.Services.Members;

    using static Data.DataConstants;

    public class MemberController : Controller
    {
        private readonly IMemberService members;

        public MemberController(IMemberService members)
        {
            this.members = members;
        }

        [Authorize]
        public IActionResult Become() => View();

        [Authorize]
        [HttpPost]
        public IActionResult Become(BecomeMemberFormModel member) 
        {
            var userId = this.User.GetId();

            var userIdAlreadyMember = this.members.UserIsMember(userId);

            if (userIdAlreadyMember)
            {
                //TODO Error message

                return BadRequest();
            }

            if (member.MemberName.Length < MemberNameMinLength || member.MemberName.Length > MemberNameMaxLength )
            {
                this.ModelState.AddModelError(nameof(member.MemberName), $"The name must be between {MemberNameMinLength} and {MemberNameMaxLength} symbols.");
            }

            // TODO Check if there should be more checks
            if (!ModelState.IsValid)
            {
                return View(member);
            }

            this.members.BecameMember(member.MemberName,member.PhoneNumber,userId);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


    }
}
