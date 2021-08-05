
namespace OperaHouseTheater.Services.Employees
{
    using System.Collections;


    public interface IEmployeeService
    {
        EmployeeQueryServiceModel BalletEmployees();

        EmployeeQueryServiceModel OperaEmployees();

        EmployeeQueryServiceModel МanagementEmployees();


    }
}
