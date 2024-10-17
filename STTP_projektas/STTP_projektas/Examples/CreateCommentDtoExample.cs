using STTP_projektas.Data.DatabaseObjects;
using Swashbuckle.AspNetCore.Filters;

namespace STTP_projektas.Examples;

public class CreateCommentDtoExample : IExamplesProvider<CreateCommentDto>
{
    public CreateCommentDto GetExamples()
    {
        return new CreateCommentDto(1,  "This is an example comment.");
    }
}