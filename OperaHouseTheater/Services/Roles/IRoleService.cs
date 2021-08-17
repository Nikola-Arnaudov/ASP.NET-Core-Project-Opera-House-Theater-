namespace OperaHouseTheater.Services.Roles
{
    public interface IRoleService
    {
        void Add(string name, int performanceId);

        int Delete(int id);

    }
}
