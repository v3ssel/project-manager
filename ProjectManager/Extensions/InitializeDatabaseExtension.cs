using Microsoft.AspNetCore.Identity;
using ProjectManager.Infrastructure.Data;

namespace ProjectManager.Web.Extensions
{
    public static class InitializeDatabaseExtension
    {
        public static async Task InitializeDatabaseAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var userManager = provider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();

                await DbInitializer.InitializeAsync(userManager, roleManager);
            };
        }
    }
}
