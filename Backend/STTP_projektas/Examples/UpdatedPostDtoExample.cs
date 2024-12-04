using STTP_projektas.Data.DatabaseObjects;
using Swashbuckle.AspNetCore.Filters;

namespace STTP_projektas.Examples;

public class UpdatedPostDtoExample : IExamplesProvider<UpdatedPostDto>
{
    public UpdatedPostDto GetExamples()
    {
        return new UpdatedPostDto("This is an example description");
    }
}