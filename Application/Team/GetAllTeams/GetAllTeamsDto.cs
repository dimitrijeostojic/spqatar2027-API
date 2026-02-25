using Application.Common;

namespace Application.Team.GetAll;

public sealed class GetAllTeamsDto : Dto
{
    public required string TeamName { get; set; }
    public string? FlagIcon { get; set; }
    public required string GroupName { get; set; }
}
