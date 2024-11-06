using STTP_projektas.Data.DatabaseObjects;
using STTP_projektas.Data.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace STTP_projektas.Examples;

public class PostDtoExample : IExamplesProvider<PostDto>
{
    public PostDto GetExamples()
    {
        return new PostDto(9, 6, "This is an example post", "This is an example description", DateTimeOffset.UtcNow);
    } 
}