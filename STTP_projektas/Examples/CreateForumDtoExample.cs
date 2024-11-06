using STTP_projektas.Data.DatabaseObjects;
using STTP_projektas.Data.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace STTP_projektas.Examples;

public class CreateForumDtoExample : IExamplesProvider<CreateForumDto>
{
    public CreateForumDto GetExamples()
    {
        return new CreateForumDto("This is an example forum title", "This is an example forum description");
    } 
}