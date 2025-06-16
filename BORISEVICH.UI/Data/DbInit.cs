using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BORISEVICH.UI.Data
{
    public class DbInit
    {
        public static async Task SetupIdentityAdmin(WebApplication application)
        {
            using var scope = application.Services.CreateScope();
            var userManager = scope
            .ServiceProvider
            .GetRequiredService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByEmailAsync("admin@gmail.com");
            if (user == null)
            {
                user = new ApplicationUser();
                await userManager.SetEmailAsync(user, "admin@gmail.com");
                await userManager.SetUserNameAsync(user, user.Email);
                user.EmailConfirmed = true;
                await userManager.CreateAsync(user, "123456");
                var claim = new Claim(ClaimTypes.Role, "admin");
                await userManager.AddClaimAsync(user, claim);
            }
        }
        public static async Task SeedData(WebApplication application)
        {
            using var scope = application.Services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Проверяем существование пользователя
            var user = await userManager.FindByEmailAsync("user@gmail.com");
            if (user == null)
            {
                user = new ApplicationUser
                {
                    Email = "user@gmail.com",
                    UserName = "user@gmail.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, "password123");

                //добавляем claim
                var claim = new Claim(ClaimTypes.Role, "user");
                await userManager.AddClaimAsync(user, claim);
            }
        }
    }
}
