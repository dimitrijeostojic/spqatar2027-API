using Core;
using MediatR;

namespace Application.Team.Update;

public sealed class UpdateTeamRequest
    : IRequest<Result<UpdateTeamResponse>>
{
    public Guid? TeamPublicId { get; set; }
    public string? TeamName { get; set; }
    public string? FlagIcon { get; set; }
    public Guid? GroupPublicId { get; set; }
}
