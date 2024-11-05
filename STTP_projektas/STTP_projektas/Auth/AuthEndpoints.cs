using Microsoft.AspNetCore.Identity;
using STTP_projektas.Auth.Model;

namespace STTP_projektas.Auth;

public static class AuthEndpoints
{
    public static void AddAuthApi(this WebApplication app)
    {
        //register
        app.MapPost("api/accounts", async (UserManager<ForumUser> userManager, RegisterUserDto dto) =>
        {
            //check user exists
            var user = await userManager.FindByNameAsync(dto.Username);
            if (user != null)
                return Results.UnprocessableEntity("Username already taken");

            var newUser = new ForumUser()
            {
                Email = dto.Email,
                UserName = dto.Username,
            };

            var createUserResult = await userManager.CreateAsync(newUser, dto.Password);
            if(!createUserResult.Succeeded)
                return Results.UnprocessableEntity();

            await userManager.AddToRoleAsync(newUser, ForumRoles.ForumUser);

            return Results.Created();
        });
        //login
        app.MapPost("api/accounts", async (UserManager<ForumUser> userManager, JwtTokenService jwtTokenService,  LoginDto dto) =>
        {
            //check user exists
            var user = await userManager.FindByNameAsync(dto.Username);
            if (user == null)
                return Results.UnprocessableEntity("Username does not exist");

            var isPasswordValid = await userManager.CheckPasswordAsync(user, dto.Password);
            if(!isPasswordValid)
                return Results.UnprocessableEntity("Username or password is incorrect");

            var roles = await userManager.GetRolesAsync(user);

            var accessToken = jwtTokenService.CreateAccessToken(user.UserName, user.Id, roles);
            
            return Results.Ok(new SucessfullLoginDto(accessToken));
        });
    }

    public record RegisterUserDto(string Username, string Email, string Password);

    public record LoginDto(string Username, string Password);
    public record SucessfullLoginDto(string AccessToken);

}