namespace OperaHouseTheater.Models.Performance
{
    using OperaHouseTheater.Data.Models;
    using System.Collections.Generic;


    public class PerformanceListingViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string PerformanceType { get; set; }

        public string Composer { get; set; }

        public string ImageUrl { get; set; }

    }
}
