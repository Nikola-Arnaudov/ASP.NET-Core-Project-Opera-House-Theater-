namespace OperaHouseTheater.Services.Performances
{
    using OperaHouseTheater.Data;
    using OperaHouseTheater.Data.Models;
    using OperaHouseTheater.Services.Events;
    using OperaHouseTheater.Services.Performances.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PerformanceService : IPerformanceService
    {
        private readonly OperaHouseTheaterDbContext data;

        public PerformanceService(OperaHouseTheaterDbContext data)
        {
            this.data = data;
        }

        public void Add(string title,
            string composer,
            string synopsis,
            string imageUrl,
            int performanceTypeId)
        {
            var newPerformance = new Performance
            {
                Title = title,
                Composer = composer,
                Synopsis = synopsis,
                ImageUrl = imageUrl,
                PerformanceTypeId = performanceTypeId
            };

            this.data.Add(newPerformance);
            this.data.SaveChanges();
        }

        public bool Edit(
            int id,
            string title,
           string composer,
           string synopsis,
           string imageUrl,
           int performanceTypeId)
        {
            var performanceData = this.data.Performances.Where(p => p.Id == id).FirstOrDefault();

            if (performanceData == null)
            {
                return false;
            }

            performanceData.Title = title;
            performanceData.Composer = composer;
            performanceData.Synopsis = synopsis;
            performanceData.ImageUrl = imageUrl;
            performanceData.PerformanceTypeId = performanceTypeId;

            this.data.SaveChanges();

            return true;
        }


        public PerformanceQueryServiceModel All(string searchTerm, string type)
        {
            var performanceQuery = this.data.Performances.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                performanceQuery = performanceQuery.Where(n =>
                    n.Title.ToLower().Contains(searchTerm.ToLower())
                    || n.Composer.ToLower().Contains(searchTerm.ToLower())
                    || n.PerformanceType.Type.ToLower().Contains(searchTerm.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(type))
            {
                performanceQuery = performanceQuery.Where(p => p.PerformanceType.Type == type);
            }

            var performances = performanceQuery
                .OrderByDescending(x => x.Id)
                .Select(p => new PerformanceServiceModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Composer = p.Composer,
                    ImageUrl = p.ImageUrl,
                    PerformanceType = p.PerformanceType.Type
                }).ToList();

            var types = this.data
                .PerformanceTypes
                .Select(t => t.Type)
                .ToList();

            return new PerformanceQueryServiceModel
            {
                Performances = performances,
                Types = types
            };
        }

        public PerformanceDetailsServiceModel Details(int id)
        {
            var crrPerformance = this.data
                .Performances
                .FirstOrDefault(p => p.Id == id);

            if (crrPerformance == null)
            {
                return null;
            }

            var performanceData = new PerformanceDetailsServiceModel
            {
                Id = crrPerformance.Id,
                Title = crrPerformance.Title,
                Composer = crrPerformance.Composer,
                Synopsis = crrPerformance.Synopsis,
                ImageUrl = crrPerformance.ImageUrl,
                Roles = this.data.RolesPerformance
                        .Where(r => r.PerformanceId == crrPerformance.Id)
                        .Select(r => new RoleServiceModel
                        {
                            Id = r.Id,
                            RoleName = r.RoleName
                        }).ToList(),
                Events = this.data.Events
                        .Where(e => e.PerformanceId == crrPerformance.Id)
                        .Select(e => new EventServiceModel
                        {
                            Id = e.Id,
                            Date = e.Date
                        })
                        .ToList(),
                Comments = this.data.Comments
                        .OrderByDescending(x => x.Id)
                        .Where(c => c.PerformanceId == crrPerformance.Id)
                        .Select(c => new CommentServiceModel
                        {
                            Id = c.Id,
                            Content = c.Content,
                            CreatorName = c.Member.MemberName,
                            CreatorId = c.MemberId
                        }).ToList()
            };

            return performanceData;
        }

        public bool Delete(int id)
        {
            var performance = data
                .Performances
                .Where(p => p.Id == id)
                .FirstOrDefault();

            if (performance == null)
            {
                return false;
            }

            var events = data.Events.Where(e => e.PerformanceId == id);

            // Check if those foreaches are working corretly
            if (events != null)
            {
                foreach (var crrEvent in events)
                {
                    var eventRoles = this.data
                            .EventRoles
                            .Where(x => x.EventId == crrEvent.Id);

                    if (eventRoles.Any())
                    {
                        foreach (var crrEventRole in eventRoles)
                        {
                            this.data.EventRoles.Remove(crrEventRole);
                        }
                    }
                }

                this.data.SaveChanges();


                data.Events.RemoveRange(events);
                //this.data.SaveChanges();
            }

            var roles = data.RolesPerformance.Where(r => r.PerformanceId == id);

            if (roles != null)
            {
                data.RolesPerformance.RemoveRange(roles);
                //this.data.SaveChanges();
            }

            var comments = data.Comments.Where(c => c.PerformanceId == id);

            if (comments != null)
            {
                data.Comments.RemoveRange(comments);
                //this.data.SaveChanges();
            }

            data.Performances.Remove(performance);
            data.SaveChanges();

            return true;
        }

        public IEnumerable<PerformanceTypeServiceModel> GetPerformanceTypes()
            => this.data
                .PerformanceTypes
                .Select(p => new PerformanceTypeServiceModel
                {
                    Id = p.Id,
                    TypeName = p.Type
                })
                .ToList();

        public bool PerformanceExistById(int id)
            => this.data.Performances.Any(p => p.Id == id);

        public PerformanceServiceModel GetPerformanceById(int id)
            => this.data.Performances
            .Where(x => x.Id == id)
            .Select(x => new PerformanceServiceModel
            {
                Id = x.Id,
                Composer = x.Composer,
                ImageUrl = x.ImageUrl,
                PerformanceType = x.PerformanceType.Type,
                PerformanceTypeId = x.PerformanceTypeId,
                Synopsis = x.Synopsis,
                Title = x.Title
            })
            .FirstOrDefault();

        public bool PerformanceTitleExist(string title)
            => this.data
            .Performances
            .Any(p => p.Title == title);
    }
}
