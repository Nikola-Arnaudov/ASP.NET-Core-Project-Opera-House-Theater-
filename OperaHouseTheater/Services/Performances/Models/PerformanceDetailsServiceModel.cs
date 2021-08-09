namespace OperaHouseTheater.Services.Performances.Models
{
    using System.Collections.Generic;

    public class PerformanceDetailsServiceModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Composer { get; set; }

        public string Synopsis { get; set; }

        public string ImageUrl { get; set; }

        public IEnumerable<RoleServiceModel> Roles { get; set; }

        public IEnumerable<EventServiceModel> Events { get; set; }

        public IEnumerable<CommentServiceModel> Comments { get; set; }
    }
}
