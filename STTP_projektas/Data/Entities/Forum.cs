using System.ComponentModel.DataAnnotations;
using STTP_projektas.Auth.Model;
using STTP_projektas.Data.DatabaseObjects;

namespace STTP_projektas.Data.Entities;

public class Forum
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    
    [Required]
    public required string UserId { get; set; }
    public ForumUser User { get; set; }
    public ForumDto ToDto()
    {
        return new ForumDto(Id, Title, Description, CreatedAt);
    }
}


