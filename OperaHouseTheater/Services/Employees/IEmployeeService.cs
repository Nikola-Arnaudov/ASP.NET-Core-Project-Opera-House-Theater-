namespace OperaHouseTheater.Services.Employees
{
    using OperaHouseTheater.Models.Employee;
    using System.Collections;
    using System.Collections.Generic;

    public interface IEmployeeService
    {
        EmployeeQueryServiceModel BalletEmployees();

        EmployeeQueryServiceModel OperaEmployees();

        EmployeeQueryServiceModel МanagementEmployees();

        EmployeeDetailsServiceModel Details(int id);

        void Add(string firstName
            ,string lastName,
            string imageUrl,
            string biography,
            int departmentId,
            int categoryId);

        bool Delete(int id);

        IEnumerable<EmployeeCategoryServiceModel> GetEmployeeCategories();

        IEnumerable<EmployeeDepartmentServiceModel> GetEmployeeDepartments();

        bool UserIsAdmin(string userId);
    }
}
