using Application.Common;

namespace Application.Group.GetGroupStandings;

public sealed class GetGroupStandingsResponse(ICollection<GetGroupStandingsDto> items)
    : EntityCollectionResult<GetGroupStandingsDto>(items)
{
}
