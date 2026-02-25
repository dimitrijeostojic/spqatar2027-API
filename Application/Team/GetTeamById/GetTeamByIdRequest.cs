using Core;
using MediatR;

namespace Application.Team.GetById;

public sealed class GetTeamByIdRequest
    : IRequest<Result<GetTeamByIdResponse>>
{
    public Guid PublicId { get; set; }
}
