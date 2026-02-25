using Core;
using Domain.Entities;
using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Group.GetGroupStandings;

public sealed class GetGroupStandingsRequestHandler(
     IMatchRepository matchRepository,
    ITeamRepository teamRepository
    )
    : IRequestHandler<GetGroupStandingsRequest, Result<GetGroupStandingsResponse>>
{
    private readonly IMatchRepository _matchRepository = matchRepository ?? throw new ArgumentNullException(nameof(matchRepository));
    private readonly ITeamRepository _teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));

    public async Task<Result<GetGroupStandingsResponse>> Handle(GetGroupStandingsRequest request, CancellationToken cancellationToken)
    {
        var teams = await _teamRepository.GetTeamsByGroupPublicId(request.GroupPublicId, cancellationToken);
        var completedMatches = await _matchRepository.GetCompletedMatches(request.GroupPublicId, cancellationToken);

        var standings = teams.ToDictionary(
            t => t.Id,
            t => new GetGroupStandingsDto
            {
                TeamPublicId = t.PublicId,
                TeamName = t.TeamName ?? string.Empty,
                Played = 0,
                Wins = 0,
                Draws = 0,
                Losses = 0,
                PointsFor = 0,
                PointsAgainst = 0,
                StandingPoints = 0
            });

        foreach (var match in completedMatches)
        {
            var delta = match.CalculateStandingsDelta();

            ApplyDelta(standings, delta.Home);
            ApplyDelta(standings, delta.Away);
        }

        var result = standings.Values
            .OrderByDescending(x => x.StandingPoints)
            .ThenByDescending(x => x.PointsFor - x.PointsAgainst)
            .ThenByDescending(x => x.PointsFor)
            .ToList();

        return Result<GetGroupStandingsResponse>.Success(new GetGroupStandingsResponse(result));
    }

    private static void ApplyDelta(
        Dictionary<int, GetGroupStandingsDto> standings,
        TeamStandingsDelta delta)
    {
        var team = standings[delta.TeamId];

        team.Played += delta.Played;
        team.Wins += delta.Wins;
        team.Draws += delta.Draws;
        team.Losses += delta.Losses;

        team.PointsFor += delta.PointsFor;
        team.PointsAgainst += delta.PointsAgainst;

        team.StandingPoints += delta.StandingPoints;
    }
}
