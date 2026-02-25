using Domain.Entities;
using Domain.RepositoryInterfaces;
using Infrastracture.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.RepositoryImplementations;

public sealed class StadiumRepository(ApplicationDbContext context) : IStadiumRepository
{
    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<List<Stadium>> GetAllStadiumsAsync(CancellationToken cancellationToken)
    {
        return await _context.Stadiums.ToListAsync(cancellationToken);
    }

    public async Task<Stadium?> GetStadiumByPublicIdAsync(Guid stadiumPublicId, CancellationToken cancellationToken)
    {
        return await _context.Stadiums.FirstOrDefaultAsync(s => s.PublicId == stadiumPublicId, cancellationToken);
    }
}
