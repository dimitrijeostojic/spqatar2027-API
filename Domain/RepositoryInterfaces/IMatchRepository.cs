using Domain.Entities;

namespace Domain.RepositoryInterfaces;

public interface IMatchRepository
{
    Task<bool> ExistsTeamConflict(int homeTeamId, int awayTeamId, DateTime startTime, CancellationToken cancellationToken);
    Task<bool> ExistsStadiumConflict(int stadiumId, DateTime startTime, CancellationToken cancellationToken);
    Task<bool> ExistsSameMatch(int homeTeamId, int awayTeamId, CancellationToken cancellationToken);
    Task<Match?> GetMatchByPublicId(Guid publicId, CancellationToken cancellationToken);
    Task<List<Match>> GetMatches(Guid? groupPublicId, CancellationToken cancellationToken);
    Task CreateMatchAsync(Match match, CancellationToken cancellationToken);
    Task<List<Match>> GetCompletedMatches(Guid groupPublicId, CancellationToken cancellationToken);
}
