using Application.Common;

namespace Application.Stadium.GetAll;

public sealed class GetAllStadiumsResponse(ICollection<GetAllStadiumsDto> items)
    : EntityCollectionResult<GetAllStadiumsDto>(items)
{
}
