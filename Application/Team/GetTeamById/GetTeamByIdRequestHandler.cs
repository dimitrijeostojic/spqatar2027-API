using Application.Common;
using Core;
using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Team.GetById;

public sealed class GetTeamByIdRequestHandler(ITeamRepository teamRepository)
    : IRequestHandler<GetTeamByIdRequest, Result<GetTeamByIdResponse>>
{
    private readonly ITeamRepository _teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));

    public async Task<Result<GetTeamByIdResponse>> Handle(GetTeamByIdRequest request, CancellationToken cancellationToken)
    {
        var team = await _teamRepository.GetByPublicIdAsync(request.PublicId, cancellationToken);
        if (team is null)
        {
            return Result<GetTeamByIdResponse>.Failure(ApplicationErrors.NotFound);
        }
        var resposne = new GetTeamByIdResponse
        {
            TeamName = team.TeamName ?? string.Empty,
            FlagIcon = team.FlagIcon,
            GroupName = team.Group?.GroupName ?? string.Empty
        };
        return Result<GetTeamByIdResponse>.Success(resposne);
    }
}
