using Core;
using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Match.GetMatches;

public sealed class GetMatchesRequestHandler(IMatchRepository matchRepository)
    : IRequestHandler<GetMatchesRequest, Result<GetMatchesResponse>>
{
    private readonly IMatchRepository _matchRepository = matchRepository ?? throw new ArgumentNullException(nameof(matchRepository));

    public async Task<Result<GetMatchesResponse>> Handle(GetMatchesRequest request, CancellationToken cancellationToken)
    {
        var matches = await _matchRepository.GetMatches(request.GroupPublicId, cancellationToken);

        var result = matches.Select(m => new GetMatchesDto
        {
            PublicId = m.PublicId,
            HomeTeam = m.HomeTeam?.TeamName ?? string.Empty,
            AwayTeam = m.AwayTeam?.TeamName ?? string.Empty,
            Stadium = m.Stadium?.StadiumName ?? string.Empty,
            StartTime = m.StartTime,
            HomePoints = m.HomePoints,
            AwayPoints = m.AwayPoints,
            Status = m.Status
        }).ToList();

        return Result<GetMatchesResponse>.Success(new GetMatchesResponse(result));
    }
}
