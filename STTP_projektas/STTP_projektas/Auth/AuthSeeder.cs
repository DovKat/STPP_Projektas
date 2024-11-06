using Microsoft.AspNetCore.Identity;
using STTP_projektas.Auth.Model;

namespace STTP_projektas.Auth;

public class AuthSeeder
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ForumUser> _userManager;

    public AuthSeeder(UserManager<ForumUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }
    
    public async Task SeedAsync()
    {
        await AddDefaultRolesASync();
        await AddAdminUserAsync();
    }

    private async Task AddDefaultRolesASync()
    {
        foreach (var role in ForumRoles.All)
        {
            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    private async Task AddAdminUserAsync()
    {
        var newAdminUser = new ForumUser()
        {
            UserName = "admin",
            Email = "admint@STTP.com"
        };

        var existAdminUser = await _userManager.FindByNameAsync(newAdminUser.UserName);
        if(existAdminUser == null)
        {
            var createUserResult = await _userManager.CreateAsync(newAdminUser, "2Uy4Fo4mW2PTaAH8gyFB");
            if (createUserResult.Succeeded)
            {
                var addToRolesResult = await _userManager.AddToRolesAsync(newAdminUser, ForumRoles.All);
                if (!addToRolesResult.Succeeded)
                {
                    Console.WriteLine($"Failed to add roles to admin: {string.Join(", ", addToRolesResult.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                Console.WriteLine($"Error creating admin user: {string.Join(", ", createUserResult.Errors.Select(e => e.Description))}");
            }
        }
    }
}