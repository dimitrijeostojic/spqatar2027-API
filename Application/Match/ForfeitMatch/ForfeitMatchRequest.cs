using Core;
using Domain.Enums;
using MediatR;

namespace Application.Match.ForfeitMatch;

public sealed class ForfeitMatchRequest : IRequest<Result<ForfeitMatchResponse>>
{
    public Guid? MatchPublicId { get; set; }
    public ForfeitSide? ForfeitLoser { get; private set; }
}
