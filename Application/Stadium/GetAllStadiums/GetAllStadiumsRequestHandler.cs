using Core;
using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Stadium.GetAll;

public sealed class GetAllStadiumsRequestHandler(IStadiumRepository stadiumRepository)
    : IRequestHandler<GetAllStadiumsRequest, Result<GetAllStadiumsResponse>>
{
    private readonly IStadiumRepository _stadiumRepository = stadiumRepository ?? throw new ArgumentNullException(nameof(stadiumRepository));

    public async Task<Result<GetAllStadiumsResponse>> Handle(GetAllStadiumsRequest request, CancellationToken cancellationToken)
    {
        var stadiums = await _stadiumRepository.GetAllStadiumsAsync(cancellationToken);
        var stadiumDtos = stadiums.Select(s => new GetAllStadiumsDto
        {
            City = s.City ?? string.Empty,
            StadiumName = s.StadiumName ?? string.Empty,
            Capacity = s.Capacity
        }).ToList();

        return Result<GetAllStadiumsResponse>.Success(new GetAllStadiumsResponse(stadiumDtos));

    }
}
