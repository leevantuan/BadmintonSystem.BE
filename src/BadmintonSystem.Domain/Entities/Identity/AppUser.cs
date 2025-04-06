using BadmintonSystem.Contract.Abstractions.Entities;
using BadmintonSystem.Domain.Enumerations;
using Microsoft.AspNetCore.Identity;

namespace BadmintonSystem.Domain.Entities.Identity;

public class AppUser : IdentityUser<Guid>, IAuditable
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string FullName { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public bool? IsDirector { get; set; }

    public bool? IsHeadOfDepartment { get; set; }

    public Guid? ManagerId { get; set; }

    public Guid PositionId { get; set; }

    public int IsReceipient { get; set; }

    public string? AvatarUrl { get; set; }

    public virtual ICollection<IdentityUserClaim<Guid>> Claims { get; set; }

    public virtual ICollection<IdentityUserLogin<Guid>> Logins { get; set; }

    public virtual ICollection<IdentityUserToken<Guid>> Tokens { get; set; }

    public virtual ICollection<AppUserRole> UserRoles { get; set; }

    //public virtual ICollection<Notification>? Notifications { get; set; }

    //public virtual ICollection<PaymentMethod>? PaymentMethods { get; set; }

    //public virtual ICollection<UserAddress>? UserAddresses { get; set; }

    //public virtual ICollection<Review>? Reviews { get; set; }

    //public virtual ICollection<Booking>? Bookings { get; set; }

    //public virtual ChatRoom? ChatRoom { get; set; }

    public GenderEnum? Gender { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }
}
