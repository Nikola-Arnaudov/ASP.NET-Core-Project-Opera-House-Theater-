namespace OperaHouseTheater.Models.Performance
{
    using System.Collections.Generic;


    public class PerformanceDetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Composer { get; set; }

        public string Synopsis { get; set; }

        public string ImageUrl { get; set; }

        public IEnumerable<RoleListingViewModel> Roles { get; set; }

        public IEnumerable<EventListingViewModel> Events { get; set; }

        public IEnumerable<CommentListingViewModel> Comments { get; set; }

    }
}
