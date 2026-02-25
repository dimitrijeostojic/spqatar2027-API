using Domain.Enums;

namespace Application.Match.GetMatches;

public sealed class GetMatchesDto
{
    public Guid PublicId { get; set; }
    public string HomeTeam { get; set; } = string.Empty;
    public string AwayTeam { get; set; } = string.Empty;
    public string Stadium { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public int? HomePoints { get; set; }
    public int? AwayPoints { get; set; }
    public bool? IsForfeit { get; set; }
    public ForfeitSide? ForfeitLoser { get; set; }
    public MatchStatusEnum Status { get; set; }
}
