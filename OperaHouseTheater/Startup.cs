namespace OperaHouseTheater
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using OperaHouseTheater.Data;
    using OperaHouseTheater.Data.Models;
    using OperaHouseTheater.Infrastructure;
    using OperaHouseTheater.Services.Admins;
    using OperaHouseTheater.Services.Comments;
    using OperaHouseTheater.Services.Employees;
    using OperaHouseTheater.Services.Events;
    using OperaHouseTheater.Services.Home;
    using OperaHouseTheater.Services.Members;
    using OperaHouseTheater.Services.News;
    using OperaHouseTheater.Services.Performances;
    using OperaHouseTheater.Services.Roles;
    using OperaHouseTheater.Services.Tickets;

    public class Startup
    {
        public Startup(IConfiguration configuration)
            => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<OperaHouseTheaterDbContext>(options => options
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services
                .AddDefaultIdentity<User>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<OperaHouseTheaterDbContext>();

            services.AddMemoryCache();

            services.AddControllersWithViews();

            services.AddTransient<INewsService, NewsService>();
            services.AddTransient<IMemberService, MemberService>();
            services.AddTransient<ITicketService, TicketService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IEventService, EventService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IAdminService, AdminService>();
            services.AddTransient<IPerformanceService, PerformanceService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IHomeService, HomeService>();



        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.PrepareDatabase();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Home/Error", "?code={0}");

            app
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "Areas",
                        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapRazorPages();
                });
        }
    }
}
