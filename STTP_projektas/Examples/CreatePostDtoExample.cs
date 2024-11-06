using STTP_projektas.Data.DatabaseObjects;
using STTP_projektas.Data.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace STTP_projektas.Examples;

public class CreatePostDtoExample : IExamplesProvider<CreatePostDto>
{
    public CreatePostDto GetExamples()
    {
        return new CreatePostDto("This is an example post", 6 , "This is an example description");
    } 
}