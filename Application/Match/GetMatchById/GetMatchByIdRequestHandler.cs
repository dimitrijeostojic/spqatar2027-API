using Application.Common;
using Core;
using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Match.GetMatchById;

public sealed class GetMatchByIdRequestHandler(IMatchRepository matchRepository)
    : IRequestHandler<GetMatchByIdRequest, Result<GetMatchByIdResponse>>
{
    private readonly IMatchRepository _matchRepository = matchRepository ?? throw new ArgumentNullException(nameof(matchRepository));

    public async Task<Result<GetMatchByIdResponse>> Handle(GetMatchByIdRequest request, CancellationToken cancellationToken)
    {
        var match = await _matchRepository.GetMatchByPublicId(request.MatchPublicId, cancellationToken);

        if (match is null)
        {
            return Result<GetMatchByIdResponse>.Failure(ApplicationErrors.NotFound);
        }

        var response = new GetMatchByIdResponse()
        {
            PublicId = match.PublicId,
            HomeTeam = match.HomeTeam?.TeamName ?? string.Empty,
            AwayTeam = match.AwayTeam?.TeamName ?? string.Empty,
            HomeTeamFlag = match.HomeTeam?.FlagIcon ?? string.Empty,
            AwayTeamFlag = match.AwayTeam?.FlagIcon ?? string.Empty,
            Stadium = match.Stadium?.StadiumName ?? string.Empty,
            City = match.Stadium?.City ?? string.Empty,
            StartTime = match.StartTime,
            HomePoints = match.HomePoints,
            AwayPoints = match.AwayPoints,
            Status = match.Status,
            IsForfeit = match.IsForfeit,
            ForfeitLoser = match.ForfeitLoser
        };

        return Result<GetMatchByIdResponse>.Success(response);
    }
}
