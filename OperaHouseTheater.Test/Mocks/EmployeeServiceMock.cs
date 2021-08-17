namespace OperaHouseTheater.Test.Mocks
{
    using Moq;
    using OperaHouseTheater.Services.Employees;

    public static class EmployeeServiceMock
    {
        public static IEmployeeService Instance 
        {
            get 
            {
                var employeeServiceMock = new Mock<IEmployeeService>();
                
                return employeeServiceMock.Object;
            }   
        }
    }
}
