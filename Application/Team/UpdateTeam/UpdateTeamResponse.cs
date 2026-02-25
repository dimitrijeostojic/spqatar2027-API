namespace Application.Team.Update;

public sealed class UpdateTeamResponse
{
    public required Guid PublicId { get; set; }
    public required string TeamName { get; set; }
    public string? FlagIcon { get; set; }
    public required string GroupName { get; set; }
}
