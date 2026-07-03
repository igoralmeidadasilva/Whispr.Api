namespace Whispr.Application.Features.V1.Users.Queries.GetById;

public sealed class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty)
                .WithError(GetUserByIdQueryErrors.UserIdIsRequired);
    }
}