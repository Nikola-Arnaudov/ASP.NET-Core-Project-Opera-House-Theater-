namespace OperaHouseTheater.Services.Performances.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PerformanceQueryServiceModel
    {
        public IEnumerable<PerformanceServiceModel> Performances { get; init; }

        public IEnumerable<string> Types { get; set; }
    }
}
