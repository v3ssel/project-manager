﻿using Microsoft.AspNetCore.Identity;

namespace ProjectManager.Infrastructure.Data
{
    public class DbInitializer
    {
        public static async Task InitializeAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await CreateUserRole(userManager, roleManager, $"{Constants.AdministratorRoleName}@gmail.com", Constants.AdministratorRoleName, "_Aa123456");
            await CreateUserRole(userManager, roleManager, $"{Constants.ManagerRoleName}@gmail.com", Constants.ManagerRoleName, "_Aa123456");
            await CreateUserRole(userManager, roleManager, $"{Constants.EmployeeRoleName}@gmail.com", Constants.EmployeeRoleName, "_Aa123456");
        }

        private static async Task CreateUserRole(UserManager<IdentityUser> userManager,
                                                 RoleManager<IdentityRole> roleManager,
                                                 string email, string roleName, string password)
        {
            var admin = new IdentityUser()
            {
                Email = email,
                UserName = email,
            };

            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            if (await userManager.FindByNameAsync(admin.Email) == null)
            {
                string adminPassword = password;

                IdentityResult result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, roleName);
                }
            }
        }
    }
}
