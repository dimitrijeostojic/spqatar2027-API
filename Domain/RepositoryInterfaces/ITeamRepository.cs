using Domain.Entities;

namespace Domain.RepositoryInterfaces;

public interface ITeamRepository
{
    Task AddAsync(Team team, CancellationToken cancellationToken = default);
    Task<List<Team>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Team?> GetByPublicIdAsync(Guid teamPublicId, CancellationToken cancellationToken);
    Team DeleteTeam(Team team, CancellationToken cancellationToken);
    Task<List<Team>> GetTeamsByGroupPublicId(Guid groupPublicId, CancellationToken cancellationToken);
}
