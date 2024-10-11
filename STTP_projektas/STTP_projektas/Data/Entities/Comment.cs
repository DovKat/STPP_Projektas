using STTP_projektas.Data.DatabaseObjects;

namespace STTP_projektas.Data.Entities;

public class Comment
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    
    public bool IsBlocked{ get; set; }
    public bool IsDeleted { get; set; }
    
    public int PostId{ get; set; }
    public CommentDto ToDto()
    {
        return new CommentDto(Id, PostId, Description, CreatedAt);
    }
}