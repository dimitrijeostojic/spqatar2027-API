namespace Application.Team.Delete;

public sealed class DeleteTeamResponse
{
    public required string TeamName { get; set; }
    public string? FlagIcon { get; set; }
    public required string GroupName { get; set; }
}
