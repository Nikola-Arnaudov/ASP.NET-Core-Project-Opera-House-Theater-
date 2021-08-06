namespace OperaHouseTheater.Services.Employees
{
    using System.Collections.Generic;

    public class EmployeeQueryServiceModel
    {
        public IEnumerable<EmployeeServiceModel> Employees { get; set; }
    }
}
