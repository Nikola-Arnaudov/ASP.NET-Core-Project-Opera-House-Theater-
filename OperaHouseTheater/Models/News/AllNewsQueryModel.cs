namespace OperaHouseTheater.Models.News
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AllNewsQueryModel
    {
        public const int NewsPerPage = 9;

        [Display(Name = "Search")]
        public string SearchTerm { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int NewsCount { get; set; }

        public IEnumerable<NewsListingViewModel> News { get; set; }
    }
}
