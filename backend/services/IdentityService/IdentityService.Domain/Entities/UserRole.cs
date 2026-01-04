using Shared.Common.Domain;

namespace IdentityService.Domain.Entities;

public class UserRole : BaseEntity
{
    public int UserId { get; private set; }
    public User User { get; private set; } = null!;
    
    public int RoleId { get; private set; }
    public Role Role { get; private set; } = null!;

    private UserRole() { } // EF Core

    public static UserRole Create(int userId, int roleId)
    {
        return new UserRole
        {
            UserId = userId,
            RoleId = roleId
        };
    }
}
