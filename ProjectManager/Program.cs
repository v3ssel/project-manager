using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Infrastructure.Data;
using ProjectManager.Infrastructure.Repositories;
using ProjectManager.Application.Services;
using ProjectManager.Application.Repositories;
using ProjectManager.Application.Services.Interfaces;
using ProjectManager.Web.Extensions;
using Application.Services.Interfaces;

namespace ProjectManager.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options => 
                        options.UseSqlServer(connectionString,
                        opt => opt.MigrationsAssembly("Infrastructure")));

            builder.Services.AddDefaultIdentity<IdentityUser>()
                            .AddRoles<IdentityRole>()
                            .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IProjectsRepository, ProjectsRepository>();
            builder.Services.AddScoped<IClientsRepository, ClientsRepository>();
            builder.Services.AddScoped<IEmployeesRepository, EmployeesRepository>();
            builder.Services.AddScoped<ITeamsRepository, TeamsRepository>();

            builder.Services.AddScoped<IProjectsService, ProjectsService>();
            builder.Services.AddScoped<IClientsService, ClientsService>();
            builder.Services.AddScoped<IEmployeesService, EmployeesService>();
            builder.Services.AddScoped<ITeamsService, TeamsService>();
            builder.Services.AddScoped<IFileManagerService, FileManagerService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            await app.InitializeDatabaseAsync();

            app.Run();
        }
    }
}