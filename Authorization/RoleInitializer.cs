using Microsoft.AspNetCore.Identity;

namespace MotqenIslamicLearningPlatform_API.Authorization
{
    public static class RoleInitializer
    {
        public static async Task InitializeAsync(IServiceProvider service)
        {
            var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { UserRoles.Admin, UserRoles.Student, UserRoles.Parent, UserRoles.Teacher };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
