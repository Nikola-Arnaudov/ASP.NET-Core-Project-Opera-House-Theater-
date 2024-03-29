﻿namespace OperaHouseTheater.Services.Performances.Models
{
    public class PerformanceServiceModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string PerformanceType { get; set; }

        public int PerformanceTypeId { get; set; }

        public string Synopsis { get; set; }

        public string Composer { get; set; }

        public string ImageUrl { get; set; }
    }
}
