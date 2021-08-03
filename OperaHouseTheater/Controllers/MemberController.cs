namespace OperaHouseTheater.Controllers.Member
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Data;
    using OperaHouseTheater.Data.Models;
    using OperaHouseTheater.Infrastructure;
    using OperaHouseTheater.Models.Member;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using static Data.DataConstants;

    public class MemberController : Controller
    {
        private readonly OperaHouseTheaterDbContext data;

        public MemberController(OperaHouseTheaterDbContext data) 
            => this.data = data;

        [Authorize]
        public IActionResult Become() => View();

        [Authorize]
        [HttpPost]
        public IActionResult Become(BecomeMemberFormModel member) 
        {
            var userId = this.User.GetId();

            var userIdAlreadyMember = this.data.Members.Any(x => x.UserId == userId);

            if (userIdAlreadyMember)
            {
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

            var memberData = new Member
            {
                MemberName = member.MemberName,
                PhoneNumber = member.PhoneNumber,
                UserId = userId
            };

            this.data.Members.Add(memberData);
            this.data.SaveChanges();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }




    }
}
