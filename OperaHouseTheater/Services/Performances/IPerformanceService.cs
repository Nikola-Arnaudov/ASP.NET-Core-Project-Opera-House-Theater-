namespace OperaHouseTheater.Services.Performances
{
    using OperaHouseTheater.Services.Performances.Models;
    using System.Collections.Generic;

    public interface IPerformanceService
    {
        PerformanceQueryServiceModel All(string searchTerm, string type);

        void Add(string title,
            string composer,
            string synopsis,
            string imageUrl,
            int performanceTypeId);

        bool Edit(
            int id,
            string title,
            string composer,
            string synopsis,
            string imageUrl,
            int performanceTypeId);

        PerformanceDetailsServiceModel Details(int id);

        bool Delete(int id);

        IEnumerable<PerformanceTypeServiceModel> GetPerformanceTypes();

        bool PerformanceExistById(int id);

        PerformanceServiceModel GetPerformanceById(int id);

        bool PerformanceTitleExist(string title);
    }
}
