namespace OperaHouseTheater.Test.Services
{
    using Xunit;
    using OperaHouseTheater.Test.Mocks;
    using Moq;
    using OperaHouseTheater.Services.Employees;
    using OperaHouseTheater.Data.Models;
    using System.Linq;

    public class EmployeeServiceTest
    {
        [Fact]
        public void DeleteShouldReturnFalseIfEmployeeNotExist()
        {
            var id = 1;

            var data = DatabaseMock.Instance;

            var employeeService = new Mock<EmployeeService>(data);
            var result = employeeService.Object.Delete(id);

            Assert.False(result);
        }

        [Fact]
        public void DeleteShouldReturnTrueIfEmployeeExist()
        {
            var id = 1;

            var data = DatabaseMock.Instance;

            data.Employees.Add(new Employee { });
            data.SaveChanges();

            var employeeService = new Mock<EmployeeService>(data);
            var result = employeeService.Object.Delete(id);

            Assert.True(result);
        }

        [Fact]
        public void GetEmployeesDepartmentsShouldActCorrectly() 
        {
            var data = DatabaseMock.Instance;

            data.Departments.Add(new Department { });
            data.SaveChanges();
            
            var employeeService = new Mock<EmployeeService>(data);

            var departments = employeeService.Object.GetEmployeeDepartments();
            var result = departments.ToList().Count();

            Assert.Equal(1, result);
        }

        [Fact]
        public void GetEmployeesCategoriesShouldActCorrectly()
        {
            var data = DatabaseMock.Instance;

            data.EmployeeCategories.Add(new EmployeeCategory { });
            data.SaveChanges();

            var employeeService = new Mock<EmployeeService>(data);

            var employeeCategories = employeeService.Object.GetEmployeeCategories();
            var result = employeeCategories.ToList().Count();

            Assert.Equal(1, result);
        }

        [Fact]
        public void GetBalletEmployeesShouldActCorrectly()
        {
            var data = DatabaseMock.Instance;

            data.Departments.Add(new Department { DepartmentName = "Балет" });
            data.SaveChanges();

            data.Employees.Add(new Employee { DepartmentId = 1 });
            data.SaveChanges();

            var commentService = new Mock<EmployeeService>(data);

            var balletEmployees = commentService.Object.BalletEmployees();
            var result = balletEmployees.Employees.ToList().Count;

            Assert.Equal(1, result);
        }

        [Fact]
        public void DetailsShouldReturnModelIfEmployeeExist()
        {
            var id = 1;

            var data = DatabaseMock.Instance;

            data.Employees.Add(new Employee { });
            data.SaveChanges();

            var commentService = new Mock<EmployeeService>(data);

            var result = commentService.Object.Details(id);

            Assert.True(result != null);
        }

        [Fact]
        public void DetailsShouldReturnNullIfEmployeeNotExist()
        {
            var id = 100;

            var data = DatabaseMock.Instance;

            data.Employees.Add(new Employee { });
            data.SaveChanges();

            var commentService = new Mock<EmployeeService>(data);

            var result = commentService.Object.Details(id);

            Assert.True(result == null);
        }

    }
}
