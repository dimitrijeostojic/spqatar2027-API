namespace Application.Stadium.GetAll;

public sealed class GetAllStadiumsDto
{
    public required string StadiumName { get; set; }
    public required string City { get; set; }
    public required int Capacity { get; set; }
}
