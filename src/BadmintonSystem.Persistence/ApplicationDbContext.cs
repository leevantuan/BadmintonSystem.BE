using System.Linq.Expressions;
using BadmintonSystem.Contract.Abstractions.Entities;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Action = BadmintonSystem.Domain.Entities.Identity.Action;

namespace BadmintonSystem.Persistence;

public sealed class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid, IdentityUserClaim<Guid>,
    AppUserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<AppUser> AppUsers { get; set; }

    public DbSet<AppRole> AppRoles { get; set; }

    public DbSet<AppUserRole> AppUserRoles { get; set; }

    public DbSet<Function> Functions { get; set; }

    public DbSet<Action> Actions { get; set; }

    public DbSet<Category> Category { get; set; }

    public DbSet<Service> Service { get; set; }

    public DbSet<PaymentType> PaymentType { get; set; }

    public DbSet<PaymentMethod> PaymentMethod { get; set; }

    public DbSet<Notification> Notification { get; set; }

    public DbSet<Gender> Gender { get; set; }

    public DbSet<Address> Address { get; set; }

    public DbSet<UserAddress> UserAddress { get; set; }

    public DbSet<ClubAddress> ClubAddress { get; set; }

    public DbSet<Club> Club { get; set; }

    public DbSet<ClubInformation> ClubInformation { get; set; }

    public DbSet<ClubImage> ClubImage { get; set; }

    public DbSet<Yard> Yard { get; set; }

    public DbSet<YardType> YardType { get; set; }

    public DbSet<Review> Review { get; set; }

    public DbSet<ReviewImage> ReviewImage { get; set; }

    public DbSet<BookingTime> BookingTime { get; set; }

    public DbSet<BookingLine> BookingLine { get; set; }

    public DbSet<TimeSlot> TimeSlot { get; set; }

    public DbSet<Booking> Booking { get; set; }

    public DbSet<Sale> Sale { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        IEnumerable<Type> softDeleteEntities = Domain.AssemblyReference.Assembly.GetTypes()
            .Where(type => typeof(ISoftDelete).IsAssignableFrom(type)
                           && type.IsClass
                           && !type.IsAbstract);

        foreach (Type softDeleteEntity in softDeleteEntities)
        {
            builder.Entity(softDeleteEntity).HasQueryFilter(GenerateQueryFilterLambda(softDeleteEntity));
        }

        builder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }

    private LambdaExpression? GenerateQueryFilterLambda(Type type)
    {
        // parameter: "w => "
        ParameterExpression parameter = Expression.Parameter(type, "w");

        // falseConstantValue: false
        ConstantExpression falseConstantValue = Expression.Constant(false);

        // propertyAccess: w.IsDeleted
        MemberExpression propertyAccess = Expression.PropertyOrField(parameter, nameof(ISoftDelete.IsDeleted));

        // equalExpression: w.IsDeleted == false
        BinaryExpression equalExpression = Expression.Equal(propertyAccess, falseConstantValue);

        // lambda: w => w.IsDeleted == false
        LambdaExpression lambda = Expression.Lambda(equalExpression, parameter);

        return lambda;
    }
}
