using IdentityService.Domain.Entities;
using Shared.Common.Domain;

namespace IdentityService.Domain.Repositories;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<List<Role>> GetRolesByIdsAsync(List<int> roleIds, CancellationToken cancellationToken = default);
}
