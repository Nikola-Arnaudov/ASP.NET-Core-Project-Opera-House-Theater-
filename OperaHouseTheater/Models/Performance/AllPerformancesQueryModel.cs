namespace OperaHouseTheater.Models.Performance
{
    using OperaHouseTheater.Services.Performances.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AllPerformancesQueryModel
    {
        public string Type { get; init; }

        [Display(Name = "Search")]
        public string SearchTerm { get; init; }

        public IEnumerable<string> Types { get; set; }

        public IEnumerable<PerformanceServiceModel> Performances { get; set; }
    }
}
