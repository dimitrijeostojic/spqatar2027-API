using Core;
using MediatR;

namespace Application.Match.GetMatchById;

public sealed class GetMatchByIdRequest : IRequest<Result<GetMatchByIdResponse>>
{
    public Guid MatchPublicId { get; set; }
}
