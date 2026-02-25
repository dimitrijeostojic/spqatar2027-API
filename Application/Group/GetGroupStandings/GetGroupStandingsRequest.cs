using Core;
using MediatR;

namespace Application.Group.GetGroupStandings;

public sealed class GetGroupStandingsRequest : IRequest<Result<GetGroupStandingsResponse>>
{
    public Guid GroupPublicId { get; set; }
}
