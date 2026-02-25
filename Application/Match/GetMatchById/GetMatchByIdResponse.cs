using Domain.Enums;

namespace Application.Match.GetMatchById;

public sealed class GetMatchByIdResponse
{
    public Guid PublicId { get; set; }

    public string HomeTeam { get; set; } = string.Empty;
    public string AwayTeam { get; set; } = string.Empty;

    public string? HomeTeamFlag { get; set; }
    public string? AwayTeamFlag { get; set; }

    public string Stadium { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;

    public DateTime StartTime { get; set; }

    public int? HomePoints { get; set; }
    public int? AwayPoints { get; set; }

    public MatchStatusEnum Status { get; set; }

    public bool IsForfeit { get; set; }
    public ForfeitSide? ForfeitLoser { get; set; }
}
