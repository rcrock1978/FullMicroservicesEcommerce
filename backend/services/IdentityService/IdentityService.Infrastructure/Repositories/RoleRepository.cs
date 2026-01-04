using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;
using IdentityService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Infrastructure.Persistence;

namespace IdentityService.Infrastructure.Repositories;

public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(IdentityDbContext context) : base(context)
    {
    }

    public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(r => r.Name == name && !r.IsDeleted, cancellationToken);
    }

    public async Task<List<Role>> GetRolesByIdsAsync(List<int> roleIds, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(r => roleIds.Contains(r.Id) && !r.IsDeleted)
            .ToListAsync(cancellationToken);
    }
}
