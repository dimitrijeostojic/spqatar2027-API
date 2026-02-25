namespace Application.Group.GetGroupStandings;

public class GetGroupStandingsDto
{
    public Guid TeamPublicId { get; set; }
    public string TeamName { get; set; } = string.Empty;

    public int Played { get; set; }
    public int Wins { get; set; }
    public int Draws { get; set; }
    public int Losses { get; set; }

    public int PointsFor { get; set; }
    public int PointsAgainst { get; set; }

    public int StandingPoints { get; set; }
}