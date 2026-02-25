using Application.Common;
using Core;
using Domain.Abstraction;
using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Match.Create;

public sealed class CreateMatchRequestHandler(
    IMatchRepository matchRepository,
    ITeamRepository teamRepository,
    IStadiumRepository stadiumRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateMatchRequest, Result<CreateMatchResponse>>
{
    public async Task<Result<CreateMatchResponse>> Handle(CreateMatchRequest request, CancellationToken cancellationToken)
    {
        var homeTeam = await teamRepository.GetByPublicIdAsync(request.HomeTeamPublicId, cancellationToken);
        var awayTeam = await teamRepository.GetByPublicIdAsync(request.AwayTeamPublicId, cancellationToken);
        var stadium = await stadiumRepository.GetStadiumByPublicIdAsync(request.StadiumPublicId, cancellationToken);

        if (homeTeam is null || awayTeam is null || stadium is null)
            return Result<CreateMatchResponse>.Failure(ApplicationErrors.NotFound);

        if (homeTeam.Id == awayTeam.Id)
            return Result<CreateMatchResponse>.Failure(ApplicationErrors.DifferentTeams);

        var hasTeamConflict = await matchRepository.ExistsTeamConflict(
            homeTeam.Id,
            awayTeam.Id,
            request.StartTime,
            cancellationToken);

        if (hasTeamConflict)
            return Result<CreateMatchResponse>.Failure(ApplicationErrors.ExistsTeamConflict);

        var hasStadiumConflict = await matchRepository.ExistsStadiumConflict(
            stadium.Id,
            request.StartTime,
            cancellationToken);

        if (hasStadiumConflict)
            return Result<CreateMatchResponse>.Failure(ApplicationErrors.ExistsStadiumConflict);

        var sameGroup = homeTeam.GroupId == awayTeam.GroupId;

        if (sameGroup)
        {
            var existsSameMatch = await matchRepository.ExistsSameMatch(
                homeTeam.Id,
                awayTeam.Id,
                cancellationToken);

            if (existsSameMatch)
                return Result<CreateMatchResponse>.Failure(ApplicationErrors.ExistsSameMatch);
        }

        var match = Domain.Entities.Match.Schedule(
            homeTeam.Id,
            awayTeam.Id,
            request.StartTime,
            stadium.Id
        );

        await matchRepository.CreateMatchAsync(match, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CreateMatchResponse>.Success(new CreateMatchResponse
        {
            PublicId = match.PublicId
        });
    }
}
