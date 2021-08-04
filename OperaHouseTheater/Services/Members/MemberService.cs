namespace OperaHouseTheater.Services.Members
{
    using OperaHouseTheater.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


    public class MemberService : IMemberService
    {
        private readonly OperaHouseTheaterDbContext data;

        public MemberService(OperaHouseTheaterDbContext data) 
            => this.data = data;

        public bool IsMember(string userId)
            => this.data
            .Members
            .Any(m => m.UserId == userId);
    }
}
