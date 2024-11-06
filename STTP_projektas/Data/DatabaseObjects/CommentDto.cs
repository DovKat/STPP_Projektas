using FluentValidation;
using STTP_projektas.Data.Entities;

namespace STTP_projektas.Data.DatabaseObjects;

public record CommentDto(int Id, int PostId, string Description, DateTimeOffset CreatedOn);

public record CreateCommentDto( int PostId, string Description)
{
    public class CreateCommentDtoValidator : AbstractValidator<CreateCommentDto>
    {
        public CreateCommentDtoValidator()
        {
            RuleFor(x => x.Description).NotEmpty().Length(min: 2, max: 500);
            RuleFor(x => x.PostId).NotNull();
        }
    }
};
public record UpdatedCommentDto(string description);