namespace OperaHouseTheater.Services.Members
{
    using System.Linq;
    using OperaHouseTheater.Data;
    using OperaHouseTheater.Data.Models;

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

        public bool UserIsMember(string userId)
            => this.data
            .Members
            .Any(m => m.UserId == userId);

        public int GetMemberId(string userId)
            => this.data
            .Members
            .Where(m => m.UserId == userId)
            .Select(m => m.Id)
            .FirstOrDefault();
    }
}
