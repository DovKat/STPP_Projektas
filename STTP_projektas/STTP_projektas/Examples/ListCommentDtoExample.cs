using STTP_projektas.Data.DatabaseObjects;

namespace STTP_projektas.Examples;
using Swashbuckle.AspNetCore.Filters;
public class ListCommentDtoExample: IExamplesProvider<List<CommentDto>>
{
    
    public List<CommentDto> GetExamples()
    {
        return new List<CommentDto>
        {
            new CommentDto( 1, 5,"This is the content of the first example Comment.", DateTimeOffset.UtcNow),
            new CommentDto( 2, 6,"This is the content of the Second example Comment.", DateTimeOffset.UtcNow),
        };
    }
}