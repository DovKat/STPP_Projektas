using System.ComponentModel.DataAnnotations;
using STTP_projektas.Auth.Model;

namespace STTP_projektas.Data.Entities;

public class Session
{
    public Guid Id { get; set; }
    public string lastRefreshToken { get; set; }
    public DateTimeOffset InitiatedAt { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    public bool isRevoked { get; set; }
    [Required]
    public required string UserId { get; set; }
    
    public ForumUser User { get; set; }

}