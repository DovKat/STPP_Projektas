using STTP_projektas.Data;
using STTP_projektas.Data.Entities;
using STTP_projektas.Helpers;

namespace STTP_projektas.Auth;

public class SessionService(SttpDbContext dbContext)
{
    public async Task CreateSessionAsync(Guid sessionId, string userId, string refreshToken, DateTime expiresAt)
    {
        dbContext.Sessions.Add(new Session
        {
            Id = sessionId,
            UserId = userId,
            InitiatedAt = DateTimeOffset.UtcNow,
            ExpiresAt = expiresAt,
            lastRefreshToken = refreshToken.ToSHA256()
        });

        await dbContext.SaveChangesAsync();
    }

    public async Task ExtendSessionAsync(Guid sessionId, string refreshToken, DateTime expiresAt)
    {
        var session = await dbContext.Sessions.FindAsync(sessionId);
        session.ExpiresAt = expiresAt;
        session.lastRefreshToken = refreshToken.ToSHA256();

        await dbContext.SaveChangesAsync();
    }
    public async Task InvalidateSessionAsync(Guid sessionId)
    {
        var session = await dbContext.Sessions.FindAsync(sessionId);
        if (session is null)
        {
            return;
        }

        session.isRevoked = true;
        await dbContext.SaveChangesAsync();
    }
    public async Task<bool> IsSessionValidAsync(Guid sessionId, string refreshToken)
    {
        var session = await dbContext.Sessions.FindAsync(sessionId);
        return session is not null && session.ExpiresAt > DateTimeOffset.UtcNow && !session.isRevoked &&
               session.lastRefreshToken == refreshToken.ToSHA256();
    }
    
}