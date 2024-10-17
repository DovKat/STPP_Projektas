using STTP_projektas.Data.DatabaseObjects;
using Swashbuckle.AspNetCore.Filters;

namespace STTP_projektas.Examples;

public class CommentDtoExample : IExamplesProvider<CommentDto>
{
    public CommentDto GetExamples()
    {
        return new CommentDto(1, 3, "This is an example comment.", DateTimeOffset.UtcNow);
    }
}