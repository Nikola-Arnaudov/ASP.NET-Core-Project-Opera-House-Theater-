namespace OperaHouseTheater.Services.Admins
{
    using OperaHouseTheater.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AdminService : IAdminService
    {
        private readonly OperaHouseTheaterDbContext data;

        public AdminService(OperaHouseTheaterDbContext data) 
            => this.data = data;

        public bool UserIsAdmin(string userId)
            => this.data
                .Admins
                .Any(x => x.UserId == userId);
    }
}
