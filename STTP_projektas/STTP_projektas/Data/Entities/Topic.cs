namespace STTP_projektas.Data.Entities;

public class Topic ()
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    
    public bool IsBlocked{ get; set; }

    public TopicDto ToDto()
    {
        return new TopicDto(Id, Title, Description, CreatedAt);
    }
}