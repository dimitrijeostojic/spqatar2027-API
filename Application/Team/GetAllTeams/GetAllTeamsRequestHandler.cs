using Application.Common;
using Core;
using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Team.GetAll;

public sealed class GetAllTeamsRequestHandler(ITeamRepository teamRepository)
    : IRequestHandler<GetAllTeamsRequest, Result<GetAllTeamsResponse>>
{
    private readonly ITeamRepository _teamRepository = teamRepository;

    public async Task<Result<GetAllTeamsResponse>> Handle(GetAllTeamsRequest request, CancellationToken cancellationToken)
    {
        var teams = await _teamRepository.GetAllAsync(cancellationToken);
        if (teams is null)
        {
            return Result<GetAllTeamsResponse>.Failure(ApplicationErrors.NotFound);
        }
        var teamDtos = teams.Select(team => new GetAllTeamsDto
        {
            PublicId = team.PublicId,
            TeamName = team.TeamName ?? string.Empty,
            FlagIcon = team.FlagIcon,
            GroupName = team.Group?.GroupName ?? string.Empty
        }).ToList();

        return Result<GetAllTeamsResponse>.Success(new GetAllTeamsResponse(teamDtos));
    }
}
