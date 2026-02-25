using Core;
using MediatR;

namespace Application.Team.Delete;

public sealed class DeleteTeamRequest
    : IRequest<Result<DeleteTeamResponse>>
{
    public required Guid PublicId { get; set; }
}
