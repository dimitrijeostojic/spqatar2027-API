using Core;
using MediatR;

namespace Application.Team.Create;

public sealed class CreateTeamRequest
    : IRequest<Result<CreateTeamResponse>>
{
    public required string TeamName { get; set; }
    public string? FlagIcon { get; set; }
    public required Guid GroupPublicId { get; set; }
}
