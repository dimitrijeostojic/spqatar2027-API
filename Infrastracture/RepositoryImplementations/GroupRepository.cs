using Domain.Entities;
using Domain.RepositoryInterfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.RepositoryImplementations;

internal sealed class GroupRepository : IGroupRepository
{
    private readonly ApplicationDbContext _context;

    public GroupRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateGroupAsync(Group group, CancellationToken cancellationToken)
    {
        await _context.Groups.AddAsync(group, cancellationToken);
    }

    public async Task<Group?> DeleteGroupAsync(Guid publicId, CancellationToken cancellationToken)
    {
        var group = await _context.Groups.FirstOrDefaultAsync(x => x.PublicId == publicId);
        if (group is not null)
        {
            _context.Groups.Remove(group);
            return group;
        }
        return null;
    }

    public async Task<List<Group>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Groups.Include(g => g.Teams).ToListAsync(cancellationToken);
    }

    public async Task<Group?> GetByPublicIdAsync(Guid publicId, CancellationToken cancellationToken = default)
    {
        return await _context.Groups.FirstOrDefaultAsync(x => x.PublicId == publicId, cancellationToken);
    }
}
