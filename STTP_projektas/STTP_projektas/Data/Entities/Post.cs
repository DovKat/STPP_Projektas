using System.ComponentModel.DataAnnotations;
using FluentValidation;
using STTP_projektas.Auth.Model;
using STTP_projektas.Data.DatabaseObjects;

namespace STTP_projektas.Data.Entities;

public class Post
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    
    public bool IsBlocked{ get; set; }
    public bool IsDeleted { get; set; }
    public int ForumId { get; set; }
    
    [Required]
    public required string UserId { get; set; }
    public ForumUser User { get; set; }
    
    public PostDto ToDto()
    {
        return new PostDto(Id, ForumId ,Title, Description, CreatedAt);
    }
}
