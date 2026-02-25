using Domain.Entities;

namespace Domain.RepositoryInterfaces;

public interface IStadiumRepository
{
    Task<List<Stadium>> GetAllStadiumsAsync(CancellationToken cancellationToken);
    Task<Stadium?> GetStadiumByPublicIdAsync(Guid stadiumPublicId, CancellationToken cancellationToken);
}
