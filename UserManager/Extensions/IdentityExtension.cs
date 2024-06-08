using Microsoft.AspNetCore.Identity;
using UserManager.Data;
using UserManager.Entities;

namespace UserManager.Extensions
{
    public static class IdentityExtension
    {
        public static IServiceCollection AddIdentification(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 1;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>();

            return services;
        }
    }
}