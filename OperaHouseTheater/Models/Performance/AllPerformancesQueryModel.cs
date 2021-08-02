namespace OperaHouseTheater.Models.Performance
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AllPerformancesQueryModel
    {
        public string Type { get; init; }

        [Display(Name = "Search")]
        public string SearchTerm { get; init; }

        public IEnumerable<string> Types { get; set; }

        public IEnumerable<PerformanceListingViewModel> Performances { get; set; }
    }
}
