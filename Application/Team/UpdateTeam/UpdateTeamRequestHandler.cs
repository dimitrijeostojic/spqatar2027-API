using Application.Common;
using Core;
using Domain.Abstraction;
using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Team.Update;

public sealed class UpdateTeamRequestHandler(ITeamRepository teamRepository, IUnitOfWork unitOfWork, IGroupRepository groupRepository)
    : IRequestHandler<UpdateTeamRequest, Result<UpdateTeamResponse>>
{
    private readonly ITeamRepository _teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IGroupRepository _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));

    public async Task<Result<UpdateTeamResponse>> Handle(UpdateTeamRequest request, CancellationToken cancellationToken)
    {
        var team = await _teamRepository.GetByPublicIdAsync(request.TeamPublicId!.Value, cancellationToken);
        if (team is null)
        {
            return Result<UpdateTeamResponse>.Failure(ApplicationErrors.NotFound);
        }
        if (request.GroupPublicId != null)
        {
            var group = await _groupRepository.GetByPublicIdAsync(request.GroupPublicId.Value, cancellationToken);
            if (group is null)
            {
                return Result<UpdateTeamResponse>.Failure(ApplicationErrors.NotFound);
            }
            team.UpdateGroup(group.Id);
        }
        if (!string.IsNullOrEmpty(request.TeamName))
        {
            team.UpdateTeamName(request.TeamName);
        }
        if (!string.IsNullOrEmpty(request.FlagIcon))
        {
            team.UpdateFlagIcon(request.FlagIcon);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var response = new UpdateTeamResponse
        {
            PublicId = team.PublicId,
            TeamName = team.TeamName ?? string.Empty,
            FlagIcon = team.FlagIcon,
            GroupName = team.Group?.GroupName ?? string.Empty,
        };
        return Result<UpdateTeamResponse>.Success(response);
    }
}
