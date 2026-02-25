using Application.Common;
using Core;
using Domain.Abstraction;
using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Match.RecordResult;

public sealed class RecordMatchResultRequestHandler(IMatchRepository matchRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<RecordMatchResultRequest, Result<RecordMatchResultResponse>>
{
    private readonly IMatchRepository _matchRepository = matchRepository ?? throw new ArgumentNullException(nameof(matchRepository));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<Result<RecordMatchResultResponse>> Handle(RecordMatchResultRequest request, CancellationToken cancellationToken = default)
    {
        var match = await _matchRepository.GetMatchByPublicId(request.MatchPublicId!.Value, cancellationToken);

        if (match is null)
            return Result<RecordMatchResultResponse>.Failure(ApplicationErrors.NotFound);

        match.RecordResult(DateTime.UtcNow, request.HomePoints, request.AwayPoints);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<RecordMatchResultResponse>.Success(new RecordMatchResultResponse()
        {
            PublicId = match.PublicId
        });
    }
}
