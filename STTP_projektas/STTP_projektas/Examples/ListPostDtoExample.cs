using STTP_projektas.Data.DatabaseObjects;

namespace STTP_projektas.Examples;
using Swashbuckle.AspNetCore.Filters;
public class ListPostDtoExample: IExamplesProvider<List<PostDto>>
{
    
    public List<PostDto> GetExamples()
    {
        return new List<PostDto>
        {
            new PostDto( 1, 2,"First Example Post", "This is the content of the first example post.", DateTimeOffset.UtcNow),
            new PostDto( 2, 5,"Second Example Post", "This is the content of the Second example post.", DateTimeOffset.UtcNow),
        };
    }
}

