using Application.Common;
using Core;
using Domain.Abstraction;
using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Team.Delete;

public sealed class DeleteTeamRequestHandler(ITeamRepository teamRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteTeamRequest, Result<DeleteTeamResponse>>
{
    private readonly ITeamRepository _teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<Result<DeleteTeamResponse>> Handle(DeleteTeamRequest request, CancellationToken cancellationToken)
    {
        var team = await _teamRepository.GetByPublicIdAsync(request.PublicId, cancellationToken);
        if (team is null)
        {
            return Result<DeleteTeamResponse>.Failure(ApplicationErrors.NotFound);
        }
        team = _teamRepository.DeleteTeam(team, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var response = new DeleteTeamResponse
        {
            TeamName = team.TeamName ?? string.Empty,
            FlagIcon = team.FlagIcon,
            GroupName = team.Group?.GroupName ?? string.Empty
        };
        return Result<DeleteTeamResponse>.Success(response);
    }
}
