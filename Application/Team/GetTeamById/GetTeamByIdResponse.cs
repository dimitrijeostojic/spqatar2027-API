namespace Application.Team.GetById;

public sealed class GetTeamByIdResponse
{
    public required string TeamName { get; set; }
    public string? FlagIcon { get; set; }
    public required string GroupName { get; set; }
}
