using Microsoft.AspNetCore.Identity;

namespace BadmintonSystem.Domain.Entities.Identity;

public class AppUserRole : IdentityUserRole<Guid>
{
    public bool IsDefault { get; set; }
}
