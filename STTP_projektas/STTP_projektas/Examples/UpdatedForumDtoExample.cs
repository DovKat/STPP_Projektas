using STTP_projektas.Data.DatabaseObjects;
using STTP_projektas.Data.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace STTP_projektas.Examples;

public class UpdatedForumDtoExample : IExamplesProvider<UpdatedForumDto>
{
    public UpdatedForumDto GetExamples()
    {
        return new UpdatedForumDto("This is an example description");
    } 
}