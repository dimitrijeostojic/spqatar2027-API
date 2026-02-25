using Core;
using MediatR;

namespace Application.Team.GetAll;

public sealed record GetAllTeamsRequest
    : IRequest<Result<GetAllTeamsResponse>>;
