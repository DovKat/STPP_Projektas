using FluentValidation;
using STTP_projektas.Data.Entities;

namespace STTP_projektas.Data.DatabaseObjects;

public record ForumDto(int Id, string Title, string Description, DateTimeOffset CreatedOn);

public record CreateForumDto(string Title,  string Description)
{
    public class CreateForumDtoValidator : AbstractValidator<CreateForumDto>
    {
        public CreateForumDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().Length(min: 2, max: 100);
            RuleFor(x => x.Description).NotEmpty().Length(min: 2, max: 500);
        }
    }
};
public record UpdatedForumDto(string description);