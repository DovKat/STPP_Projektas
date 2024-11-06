using STTP_projektas.Data.DatabaseObjects;
using STTP_projektas.Data.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace STTP_projektas.Examples;

public class ForumDtoExample : IExamplesProvider<ForumDto>
{
    public ForumDto GetExamples()
    {
        return new ForumDto(5, "This is an example forum", "This is an example description", DateTimeOffset.UtcNow);
    } 
}