using Application.Common;
using Core;
using Domain.Abstraction;
using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Team.Create;

public sealed class CreateTeamRequestHandler(IGroupRepository groupRepository, ITeamRepository teamRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateTeamRequest, Result<CreateTeamResponse>>
{
    private readonly IGroupRepository _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
    private readonly ITeamRepository _teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<Result<CreateTeamResponse>> Handle(CreateTeamRequest request, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetByPublicIdAsync(request.GroupPublicId);
        if (group is null)
        {
            return Result<CreateTeamResponse>.Failure(ApplicationErrors.NotFound);
        }

        var createdTeam = Domain.Entities.Team.Create(request.TeamName, request.FlagIcon, group);
        await _teamRepository.AddAsync(createdTeam);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var response = new CreateTeamResponse
        {
            TeamName = createdTeam.TeamName ?? string.Empty,
            FlagIcon = createdTeam.FlagIcon
        };

        return Result<CreateTeamResponse>.Success(response);
    }
}
