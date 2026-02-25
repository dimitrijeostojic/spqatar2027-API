using Core;
using MediatR;

namespace Application.Match.GetMatches;

public sealed class GetMatchesRequest : IRequest<Result<GetMatchesResponse>>
{
    public Guid? GroupPublicId { get; set; }
}
