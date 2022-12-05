using Guitaria;
using Guitaria.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using static Guitaria.Data.Constants.ConstantValues.SeedData;
namespace Guitaria.Services.Services
{
    public static class SeedData
    {
        public static void InitializeDatabase(WebApplication webApplication)
        {
            using (IServiceScope serviceScope = webApplication.Services.CreateScope())
            {
                var testInitializeAccounts = serviceScope.ServiceProvider;
                try
                {
                    InitializeAccounts(testInitializeAccounts).Wait();
                }
                catch (Exception e)
                {
                    ILogger<StartUp> logger = testInitializeAccounts.GetRequiredService<ILogger<StartUp>>();
                    logger.LogError(e, "Couldn't seed database.");
                }
            }
                
        }
        public static async Task InitializeAccounts(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            await CreateRoleAsync(roleManager);

            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            await CreateAdmin(userManager);
        }

        private static async Task CreateRoleAsync(RoleManager<IdentityRole<Guid>> roleManager)
        {
            bool adminRoleExists = await roleManager.RoleExistsAsync(AdministratorRole);
            bool userRoleExists = await roleManager.RoleExistsAsync(UserRole);

            if(adminRoleExists || userRoleExists)
            {
                return;
            }
            await roleManager.CreateAsync(new IdentityRole<Guid>(AdministratorRole));
            await roleManager.CreateAsync(new IdentityRole<Guid>(UserRole));
        }

        private static async Task CreateAdmin(UserManager<User> userManager)
        {
            User testAdmin = await userManager.FindByEmailAsync(AdminEmail);
            if(testAdmin != null)
            {
                return;
            }
            testAdmin = new User()
            {
                UserName = AdminUserName,
                Email = AdminEmail
            };
            await userManager.CreateAsync(testAdmin, AdminPassword);
            await userManager.AddToRoleAsync(testAdmin,AdministratorRole);
        }

    }
}
