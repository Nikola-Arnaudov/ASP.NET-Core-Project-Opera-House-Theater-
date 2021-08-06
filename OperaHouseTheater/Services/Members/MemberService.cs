namespace OperaHouseTheater.Services.Members
{
    using OperaHouseTheater.Data;
    using OperaHouseTheater.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


    public class MemberService : IMemberService
    {
        private readonly OperaHouseTheaterDbContext data;

        public MemberService(OperaHouseTheaterDbContext data) 
            => this.data = data;

        public void BecameMember(string memberName, string phoneNumber, string userId)
        {
            var memberData = new Member
            {
                MemberName = memberName,
                PhoneNumber = phoneNumber,
                UserId = userId
            };

            this.data.Members.Add(memberData);
            this.data.SaveChanges();
        }

        public bool IsMember(string userId)
            => this.data
            .Members
            .Any(m => m.UserId == userId);
    }
}
