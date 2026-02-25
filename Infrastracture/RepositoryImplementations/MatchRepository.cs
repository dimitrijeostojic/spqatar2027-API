using Domain.Entities;
using Domain.Enums;
using Domain.RepositoryInterfaces;
using Infrastracture.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.RepositoryImplementations;

public sealed class MatchRepository(ApplicationDbContext context) : IMatchRepository
{
    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<bool> ExistsSameMatch(int homeTeamId, int awayTeamId, CancellationToken cancellationToken)
    {
        var existsSameMatch = await _context.Matches.FirstOrDefaultAsync(m => m.HomeTeamId == homeTeamId && m.AwayTeamId == awayTeamId, cancellationToken);
        if (existsSameMatch != null)
        {
            return true;
        }
        return false;
    }

    public async Task<bool> ExistsStadiumConflict(int stadiumId, DateTime startTime, CancellationToken cancellationToken = default)
    {
        var existsStadium = await _context.Matches.FirstOrDefaultAsync(m => m.StadiumId == stadiumId && m.StartTime == startTime, cancellationToken);
        if (existsStadium == null)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> ExistsTeamConflict(int homeTeamId, int awayTeamId, DateTime startTime, CancellationToken cancellationToken = default)
    {
        var existsHomeTeam = await _context.Matches.FirstOrDefaultAsync(m => m.HomeTeamId == homeTeamId && m.StartTime == startTime, cancellationToken);
        var existsAwayTeam = await _context.Matches.FirstOrDefaultAsync(m => m.AwayTeamId == awayTeamId && m.StartTime == startTime, cancellationToken);
        if (existsHomeTeam != null || existsAwayTeam != null)
        {
            return true;
        }
        return false;
    }

    public async Task<Match?> GetMatchByPublicId(Guid publicId, CancellationToken cancellationToken)
    {
        return await _context.Matches
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Stadium)
            .FirstOrDefaultAsync(m => m.PublicId == publicId, cancellationToken);
    }

    public async Task<List<Match>> GetMatches(Guid? groupPublicId, CancellationToken cancellationToken)
    {
        var query = _context.Matches.AsQueryable();

        if (groupPublicId.HasValue)
        {
            query = query.Where(m =>
                m.HomeTeam!.Group!.PublicId == groupPublicId &&
                m.AwayTeam!.Group!.PublicId == groupPublicId);
        }

        return await query
            .Include(m => m.HomeTeam)
                .ThenInclude(t => t!.Group)
            .Include(m => m.AwayTeam)
                .ThenInclude(t => t!.Group)
            .Include(m => m.Stadium)
            .OrderBy(m => m.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task CreateMatchAsync(Match match, CancellationToken cancellationToken)
    {
        await _context.Matches.AddAsync(match, cancellationToken);
    }

    public async Task<List<Match>> GetCompletedMatches(Guid groupPublicId, CancellationToken cancellationToken)
    {
        var query = _context.Matches.AsQueryable();
        return await query.Where(m =>
                m.Status == MatchStatusEnum.Completed &&
                m.HomeTeam!.Group!.PublicId == groupPublicId &&
                m.AwayTeam!.Group!.PublicId == groupPublicId)
            .ToListAsync(cancellationToken);
    }
}
