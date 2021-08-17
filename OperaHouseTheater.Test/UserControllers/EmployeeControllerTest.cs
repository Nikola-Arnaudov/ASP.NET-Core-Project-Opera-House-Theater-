namespace OperaHouseTheater.Test.UserControllers
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using OperaHouseTheater.Controllers.Employee;
    using OperaHouseTheater.Services.Employees;

    public class EmployeeControllerTest
    {
        [Fact]
        public void BalletEmployeesShouldReturnView()
            => MyController<EmployeeController>
                .Instance()
                .Calling(c => c.BalletEmployees())
                .ShouldReturn()
                .View(v => v.WithModelOfType<EmployeeQueryServiceModel>());

        [Fact]
        public void OperaEmployeesShouldReturnView()
            => MyController<EmployeeController>
                .Instance()
                .Calling(c => c.OperaEmployees())
                .ShouldReturn()
                .View(v => v.WithModelOfType<EmployeeQueryServiceModel>());

        [Fact]
        public void ManagementEmployeesShouldReturnView()
            => MyController<EmployeeController>
                .Instance()
                .Calling(c => c.ManagementEmployees())
                .ShouldReturn()
                .View(v => v.WithModelOfType<EmployeeQueryServiceModel>());
    }
}
