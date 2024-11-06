using STTP_projektas.Data.DatabaseObjects;
using Swashbuckle.AspNetCore.Filters;

namespace STTP_projektas.Examples;

public class UpdatedCommentDtoExample : IExamplesProvider<UpdatedCommentDto>
{
    public UpdatedCommentDto GetExamples()
    {
        return new UpdatedCommentDto( "This is an example comment.");
    }
}