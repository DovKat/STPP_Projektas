using STTP_projektas.Data.DatabaseObjects;
using STTP_projektas.Data.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace STTP_projektas.Examples;

public class PostDtoExample : IExamplesProvider<PostDto>
{
    public PostDto GetExamples()
    {
        return null; /*  new PostDto
        {
            Id = 1,
            Forum =
            Title = "Example Post",
            Description = "This is an example post description."
        };  */
    } 
}