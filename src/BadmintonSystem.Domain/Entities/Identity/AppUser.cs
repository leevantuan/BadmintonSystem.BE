using Microsoft.AspNetCore.Identity;

namespace BadmintonSystem.Domain.Entities.Identity;
public class AppUser : IdentityUser<Guid>
{
    public AppUser()
    {
        IsDirector = false;
        IsHeadOfDepartment = false;
        FullName = FirstName + LastName;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public DateTime? DayOfBirth { get; set; }
    public bool IsDirector { get; set; }
    public bool IsHeadOfDepartment { get; set; }
    public string? ManagerId { get; set; }
    public string? PositionId { get; set; }
    public int? IsRecipient { get; set; }

    public virtual ICollection<IdentityUserClaim<Guid>> Claims { get; set; }
    public virtual ICollection<IdentityUserLogin<Guid>> Logins { get; set; }
    public virtual ICollection<IdentityUserToken<Guid>> Tokens { get; set; }
    public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; }
    public virtual ICollection<PermissionInUser> PermissionInUsers { get; set; }
}
