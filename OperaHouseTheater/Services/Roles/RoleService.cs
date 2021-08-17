namespace OperaHouseTheater.Services.Roles
{
    using System.Linq;
    using OperaHouseTheater.Data;
    using OperaHouseTheater.Data.Models;

    public class RoleService : IRoleService
    {
        private readonly OperaHouseTheaterDbContext data;

        public RoleService(OperaHouseTheaterDbContext data) 
            => this.data = data;

        public void Add(string name,int performanceId)
        {
            var roleData = new Role
            {
                RoleName = name,
                PerformanceId = performanceId
            };

            this.data.RolesPerformance.Add(roleData);
            this.data.SaveChanges();
        }

        public int Delete(int id) 
        {
            var role = this.data
                .RolesPerformance
                .Where(r => r.Id == id)
                .FirstOrDefault();

            if (role == null)
            {
                return 0;
            }

            var performanceId = role.PerformanceId;

            this.data.RolesPerformance.Remove(role);
            this.data.SaveChanges();

            return performanceId;
        }
    }
}
