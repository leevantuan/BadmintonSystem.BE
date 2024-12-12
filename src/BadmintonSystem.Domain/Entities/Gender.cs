using BadmintonSystem.Contract.Abstractions.Entities;
using BadmintonSystem.Domain.Entities.Identity;

namespace BadmintonSystem.Domain.Entities;
public class Gender : EntityAuditBase<Guid>
{
    public string Name { get; set; }

    public virtual ICollection<AppUser>? Users { get; set; }
}
