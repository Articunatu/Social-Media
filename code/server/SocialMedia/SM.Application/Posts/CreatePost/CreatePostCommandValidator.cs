using FluentValidation;

namespace SM.Application.Posts.CreatePost;

internal class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(x => x.AuthorId)
            .NotEqual(Guid.Empty).WithMessage("AuthorMissing");

        RuleFor(x => x.Content)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("ContentEmpty")
            .MaximumLength(300).WithMessage("ContentMaxLength");
    }
}
