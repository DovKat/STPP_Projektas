using FluentValidation;
using STTP_projektas.Data.Entities;

namespace STTP_projektas.Data.DatabaseObjects;

public record PostDto(int Id, int Forum, string Title, string Description, DateTimeOffset CreatedOn);

public record CreatePostDto(string Title, int ForumId, string Description)
{
    public class CreatePostDtoValidator : AbstractValidator<CreatePostDto>
    {
        public CreatePostDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().Length(min: 2, max: 100);
            RuleFor(x => x.Description).NotEmpty().Length(min: 2, max: 500);
            RuleFor(x => x.ForumId).NotNull();
        }
    }
};
public record UpdatedPostDto(string description);