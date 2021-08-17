namespace OperaHouseTheater.Test.Mocks
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using OperaHouseTheater.Data;

    public class DatabaseMock
    {
        public static OperaHouseTheaterDbContext Instance 
        {
            get 
            {
                var dbContextOptions = new DbContextOptionsBuilder<OperaHouseTheaterDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                return new OperaHouseTheaterDbContext(dbContextOptions);
            }
        }
    }
}
