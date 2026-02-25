using Application.Common;

namespace Application.Team.GetAll;

public sealed class GetAllTeamsResponse(ICollection<GetAllTeamsDto> items)
    : EntityCollectionResult<GetAllTeamsDto>(items)
{
}
