using Application.Common;
using Core;
using Domain.Abstraction;
using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Match.ForfeitMatch;

public sealed class ForfeitMatchRequestHandler(IMatchRepository matchRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<ForfeitMatchRequest, Result<ForfeitMatchResponse>>
{
    private readonly IMatchRepository _matchRepository = matchRepository ?? throw new ArgumentNullException(nameof(matchRepository));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<Result<ForfeitMatchResponse>> Handle(ForfeitMatchRequest request, CancellationToken cancellationToken)
    {
        var match = await _matchRepository.GetMatchByPublicId(request.MatchPublicId!.Value, cancellationToken);
        if (match is null)
        {
            return Result<ForfeitMatchResponse>.Failure(ApplicationErrors.NotFound);
        }
        match.Forfeit(request.ForfeitLoser!.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var response = new ForfeitMatchResponse
        {
            PubliId = match.PublicId
        };
        return Result<ForfeitMatchResponse>.Success(response);
    }
}
