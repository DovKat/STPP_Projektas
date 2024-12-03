using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using STTP_projektas.Auth.Model;
using STTP_projektas.Data.Entities;

namespace STTP_projektas.Auth;

public static class AuthEndpoints
{
    public static void AddAuthApi(this WebApplication app)
    {
        //register
        app.MapPost("api/register", async (UserManager<ForumUser> userManager, RegisterUserDto dto) =>
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
        app.MapPost("api/login", async (UserManager<ForumUser> userManager, JwtTokenService jwtTokenService, SessionService sessionService,  HttpContext httpContext, LoginDto dto) =>
        {
            //check user exists
            var user = await userManager.FindByNameAsync(dto.Username);
            if (user == null)
                return Results.UnprocessableEntity("Username does not exist");

            var isPasswordValid = await userManager.CheckPasswordAsync(user, dto.Password);
            if(!isPasswordValid)
                return Results.UnprocessableEntity("Username or password is incorrect");

            var roles = await userManager.GetRolesAsync(user);

            var sessionId = Guid.NewGuid();
            var expiresAt = DateTime.UtcNow.AddDays(3);
            var accessToken = jwtTokenService.CreateAccessToken(user.UserName, user.Id, roles);
            var refreshToken = jwtTokenService.CreateRefreshToken(sessionId, user.Id, expiresAt);

            await sessionService.CreateSessionAsync(sessionId, user.Id, refreshToken, expiresAt);
            
            var cookieOptions = new CookieOptions
            {
                HttpOnly = false,
                SameSite = SameSiteMode.None, 
                Expires = expiresAt,
                Secure = true
            };
            
            httpContext.Response.Cookies.Append("RefreshToken", refreshToken, cookieOptions);
            
            return Results.Ok(new SucessfullLoginDto(accessToken));
        });

        app.MapPost("api/accessToken", async (UserManager<ForumUser> userManager, SessionService sessionService, JwtTokenService jwtTokenService, HttpContext httpContext) =>
        {
            if (!httpContext.Request.Cookies.TryGetValue("RefreshToken", out var refreshToken))
            {
                return Results.UnprocessableEntity();
            }
            
            if (!jwtTokenService.TryParseRefreshToken(refreshToken, out var claims))
            {
                return Results.UnprocessableEntity();
            }

            var sessionId = claims.FindFirstValue("SessionId");
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                return Results.UnprocessableEntity();
            }

            var sessionIdAsGuid = Guid.Parse(sessionId);
            if (!await sessionService.IsSessionValidAsync(sessionIdAsGuid, refreshToken))
            {
                return Results.UnprocessableEntity();
            }
            
            var userId = claims.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return Results.UnprocessableEntity();
            }

            var roles = await userManager.GetRolesAsync(user);
            
            var expiresAt = DateTime.UtcNow.AddDays(3);
            var accessToken = jwtTokenService.CreateAccessToken(user.UserName, user.Id, roles);
            var newRefreshToken = jwtTokenService.CreateRefreshToken(sessionIdAsGuid, user.Id, expiresAt);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = false,
                SameSite = SameSiteMode.None, 
                Expires = expiresAt,
                Secure = true
            };
            
            httpContext.Response.Cookies.Append("RefreshToken", newRefreshToken, cookieOptions);

            await sessionService.ExtendSessionAsync(sessionIdAsGuid, newRefreshToken, expiresAt);
            
            return Results.Ok(new SucessfullLoginDto(accessToken));
        });
        
        
        app.MapPost("api/logout", async (SessionService sessionService, JwtTokenService jwtTokenService, HttpContext httpContext) =>
        {
            if (!httpContext.Request.Cookies.TryGetValue("RefreshToken", out var refreshToken))
            {
                return Results.UnprocessableEntity();
            }
            
            if (!jwtTokenService.TryParseRefreshToken(refreshToken, out var claims))
            {
                return Results.UnprocessableEntity();
            }

            var sessionId = claims.FindFirstValue("SessionId");
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                return Results.UnprocessableEntity();
            }

            await sessionService.InvalidateSessionAsync(Guid.Parse(sessionId));
            httpContext.Response.Cookies.Delete("RefreshToken");
            
            return Results.Ok();
        });
    }

    public record RegisterUserDto(string Username, string Email, string Password);

    public record LoginDto(string Username, string Password);
    public record SucessfullLoginDto(string AccessToken);

}