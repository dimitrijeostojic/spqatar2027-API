using Core;
using MediatR;

namespace Application.Match.Create;

public sealed class CreateMatchRequest : IRequest<Result<CreateMatchResponse>>
{
    public Guid HomeTeamPublicId { get; set; }
    public Guid AwayTeamPublicId { get; set; }
    public Guid StadiumPublicId { get; set; }
    public DateTime StartTime { get; set; }
}
