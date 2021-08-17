namespace OperaHouseTheater.Services.Admins
{
    using System.Linq;
    using OperaHouseTheater.Data;

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
