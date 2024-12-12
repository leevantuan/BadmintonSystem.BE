using BadmintonSystem.Contract.Abstractions.Entities;
using Microsoft.AspNetCore.Identity;

namespace BadmintonSystem.Domain.Entities.Identity;

public class AppRole : IdentityRole<Guid>, IAuditable
{
    public string Description { get; set; }

    public string RoleCode { get; set; }

    public virtual ICollection<AppUserRole> UserRoles { get; set; }

    public virtual ICollection<IdentityRoleClaim<Guid>> Claims { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }
}
