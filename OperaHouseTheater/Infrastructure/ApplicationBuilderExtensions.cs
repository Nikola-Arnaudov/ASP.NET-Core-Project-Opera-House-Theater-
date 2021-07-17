namespace OperaHouseTheater.Infrastructure
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using OperaHouseTheater.Data;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app) 
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

           var data = scopedServices.ServiceProvider.GetService<OperaHouseTheaterDbContext>();

            data.Database.Migrate();

            return app;
        }


    }
}
