using Core;
using MediatR;

namespace Application.Match.RecordResult;

public sealed class RecordMatchResultRequest : IRequest<Result<RecordMatchResultResponse>>
{
    public Guid? MatchPublicId { get; set; }
    public int HomePoints { get; set; }
    public int AwayPoints { get; set; }
}
