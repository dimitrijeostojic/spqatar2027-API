using Application.Common;

namespace Application.Match.GetMatches;

public sealed class GetMatchesResponse(ICollection<GetMatchesDto> items)
    : EntityCollectionResult<GetMatchesDto>(items)
{
}
