using Domain.Enums;

namespace Domain.Entities;

public class Match : Entity
{
    public int HomeTeamId { get; private set; }
    public int AwayTeamId { get; private set; }
    public DateTime StartTime { get; private set; }
    public MatchStatusEnum Status { get; private set; }
    public int StadiumId { get; private set; }
    public int? HomePoints { get; private set; }
    public int? AwayPoints { get; private set; }
    public bool IsForfeit { get; private set; }
    public ForfeitSide? ForfeitLoser { get; private set; }
    public Team? HomeTeam { get; private set; }
    public Team? AwayTeam { get; private set; }
    public Stadium? Stadium { get; private set; }

    private const int _defaultForfeitWinnerPoints = 20;
    private const int _defaultForfeitLoserPoints = 0;

    private const int _winStandingPoints = 2;
    private const int _lossStandingPoints = 1;

    public static Match Schedule(int homeTeamId, int awayTeamId, DateTime startTime, int stadiumId)
    {
        if (homeTeamId <= 0) throw new ArgumentException("HomeTeamId is required.", nameof(homeTeamId));
        if (awayTeamId <= 0) throw new ArgumentException("AwayTeamId is required.", nameof(awayTeamId));
        if (stadiumId <= 0) throw new ArgumentException("StadiumId is required.", nameof(stadiumId));
        if (homeTeamId == awayTeamId) throw new InvalidOperationException("Home and away team must be different.");

        if (startTime == default) throw new ArgumentException("StartTime is required.", nameof(startTime));

        return new Match
        {
            HomeTeamId = homeTeamId,
            AwayTeamId = awayTeamId,
            StadiumId = stadiumId,
            StartTime = startTime,
            Status = MatchStatusEnum.Scheduled
        };
    }

    public void RecordResult(DateTime now, int homePoints, int awayPoints)
    {
        EnsureNotFinishedOrCancelled();

        if (now < StartTime)
            throw new InvalidOperationException("Cannot record result before StartTime (except Forfeit).");

        if (homePoints < 0 || awayPoints < 0)
            throw new InvalidOperationException("Points cannot be negative.");

        if (Status is not (MatchStatusEnum.Scheduled))
            throw new InvalidOperationException("Result can only be recorded for Scheduled/InProgress matches.");

        HomePoints = homePoints;
        AwayPoints = awayPoints;

        IsForfeit = false;
        ForfeitLoser = null;

        Status = MatchStatusEnum.Completed;
    }

    public void Forfeit(ForfeitSide loserSide)
    {
        EnsureNotFinishedOrCancelled();

        IsForfeit = true;
        ForfeitLoser = loserSide;

        if (loserSide == ForfeitSide.Home)
        {
            HomePoints = _defaultForfeitLoserPoints;
            AwayPoints = _defaultForfeitWinnerPoints;
        }
        else
        {
            HomePoints = _defaultForfeitWinnerPoints;
            AwayPoints = _defaultForfeitLoserPoints;
        }

        Status = MatchStatusEnum.Completed;
    }

    public MatchStandingsDelta CalculateStandingsDelta()
    {
        if (Status != MatchStatusEnum.Completed)
            throw new InvalidOperationException("Standings delta can only be calculated for finished matches.");

        if (HomePoints is null || AwayPoints is null)
            throw new InvalidOperationException("Finished match must have points.");

        var homeFor = HomePoints.Value;
        var homeAgainst = AwayPoints.Value;
        var awayFor = AwayPoints.Value;
        var awayAgainst = HomePoints.Value;

        var homeWin = homeFor > homeAgainst;
        var awayWin = awayFor > awayAgainst;
        var draw = homeFor == homeAgainst;

        var homeStandingPoints = homeWin ? _winStandingPoints : _lossStandingPoints;
        var awayStandingPoints = awayWin ? _winStandingPoints : _lossStandingPoints;

        return new MatchStandingsDelta(
            Home: new TeamStandingsDelta(
                TeamId: HomeTeamId,
                Played: 1,
                Wins: homeWin ? 1 : 0,
                Draws: draw ? 1 : 0,
                Losses: (!homeWin && !draw) ? 1 : 0,
                PointsFor: homeFor,
                PointsAgainst: homeAgainst,
                StandingPoints: homeStandingPoints
            ),
            Away: new TeamStandingsDelta(
                TeamId: AwayTeamId,
                Played: 1,
                Wins: awayWin ? 1 : 0,
                Draws: draw ? 1 : 0,
                Losses: (!awayWin && !draw) ? 1 : 0,
                PointsFor: awayFor,
                PointsAgainst: awayAgainst,
                StandingPoints: awayStandingPoints
            )
        );
    }

    private void EnsureNotFinishedOrCancelled()
    {
        if (Status is MatchStatusEnum.Completed or MatchStatusEnum.Cancelled)
            throw new InvalidOperationException("Match is already finished/cancelled.");
    }
}

public sealed record MatchStandingsDelta(TeamStandingsDelta Home, TeamStandingsDelta Away);

public sealed record TeamStandingsDelta(
    int TeamId,
    int Played,
    int Wins,
    int Draws,
    int Losses,
    int PointsFor,
    int PointsAgainst,
    int StandingPoints
);

