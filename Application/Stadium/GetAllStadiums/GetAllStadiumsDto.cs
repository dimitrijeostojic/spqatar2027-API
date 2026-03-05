using Application.Common;

namespace Application.Stadium.GetAll;

public sealed class GetAllStadiumsDto : Dto
{
    public required string StadiumName { get; set; }
    public required string City { get; set; }
    public required int Capacity { get; set; }
}
