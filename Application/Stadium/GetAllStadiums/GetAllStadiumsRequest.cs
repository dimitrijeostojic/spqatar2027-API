using Core;
using MediatR;

namespace Application.Stadium.GetAll;

public sealed class GetAllStadiumsRequest
    : IRequest<Result<GetAllStadiumsResponse>>
{
}
