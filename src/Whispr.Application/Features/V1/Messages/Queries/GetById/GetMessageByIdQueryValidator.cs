namespace Whispr.Application.Features.V1.Messages.Queries.GetById;

public sealed class GetMessageByIdQueryValidator : AbstractValidator<GetMessageByIdQuery>
{
    public GetMessageByIdQueryValidator()
    {
        RuleFor(x => x.MessageId)
            .NotEqual(Guid.Empty)
                .WithError(GetMessageByIdQueryErrors.MessageIdIsRequired);
    }
}