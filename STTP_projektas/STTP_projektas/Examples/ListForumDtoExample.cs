using STTP_projektas.Data.DatabaseObjects;

namespace STTP_projektas.Examples;
using Swashbuckle.AspNetCore.Filters;
public class ListForumDtoExample: IExamplesProvider<List<ForumDto>>
{
    
    public List<ForumDto> GetExamples()
    {
        return new List<ForumDto>
        {
            new ForumDto( 1, "First Example Forum", "This is the content of the first example Forum.", DateTimeOffset.UtcNow),
            new ForumDto( 2, "Second Example Forum", "This is the content of the Second example Forum.", DateTimeOffset.UtcNow),
        };
    }
}