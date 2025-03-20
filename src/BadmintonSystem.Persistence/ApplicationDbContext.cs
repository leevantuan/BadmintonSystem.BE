using System.Linq.Expressions;
using BadmintonSystem.Contract.Abstractions.Entities;
using BadmintonSystem.Contract.Abstractions.Services;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Action = BadmintonSystem.Domain.Entities.Identity.Action;
using DayOfWeek = BadmintonSystem.Domain.Entities.DayOfWeek;

namespace BadmintonSystem.Persistence;

public sealed class ApplicationDbContext
    : IdentityDbContext<AppUser, AppRole, Guid, IdentityUserClaim<Guid>,
        AppUserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    private readonly ICurrentTenantService _currentTenantService;

    public string CurrentConnectionString { get; set; }

    public ApplicationDbContext
        (DbContextOptions<ApplicationDbContext> options, ICurrentTenantService currentTenantService)
        : base(options)
    {
        _currentTenantService = currentTenantService;
        CurrentConnectionString = _currentTenantService.ConnectionString;
    }

    public DbSet<AppUser> AppUsers { get; set; }

    public DbSet<AppRole> AppRoles { get; set; }

    public DbSet<AppUserRole> AppUserRoles { get; set; }

    public DbSet<Function> Functions { get; set; }

    public DbSet<Action> Actions { get; set; }

    public DbSet<Category> Category { get; set; }

    public DbSet<Service> Service { get; set; }

    public DbSet<PaymentMethod> PaymentMethod { get; set; }

    public DbSet<Notification> Notification { get; set; }

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

    public DbSet<BookingLine> BookingLine { get; set; }

    public DbSet<TimeSlot> TimeSlot { get; set; }

    public DbSet<Booking> Booking { get; set; }

    public DbSet<Sale> Sale { get; set; }

    public DbSet<Price> Price { get; set; }

    public DbSet<YardPrice> YardPrice { get; set; }

    public DbSet<DayOff> DayOff { get; set; }

    public DbSet<DayOfWeek> DayOfWeek { get; set; }

    public DbSet<FixedSchedule> FixedSchedule { get; set; }

    public DbSet<TimeSlotOfWeek> TimeSlotOfWeek { get; set; }

    public DbSet<ComboFixed> ComboFixed { get; set; }

    public DbSet<ServiceLine> ServiceLine { get; set; }

    public DbSet<Bill> Bill { get; set; }

    public DbSet<BillLine> BillLine { get; set; }

    public DbSet<Provider> Provider { get; set; }

    public DbSet<InventoryReceipt> InventoryReceipt { get; set; }

    public DbSet<OriginalQuantity> OriginalQuantity { get; set; }

    public DbSet<ChatRoom> ChatRoom { get; set; }

    public DbSet<ChatMessage> ChatMessage { get; set; }

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

    // COnfig connection string
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = _currentTenantService.ConnectionString;

        if (string.IsNullOrEmpty(connectionString))
        {
            // Sử dụng cấu hình từ file appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();

            connectionString = configuration.GetConnectionString("PostgresConnectionStrings");
        }

        if (!string.IsNullOrEmpty(connectionString))
        {
            optionsBuilder.UseNpgsql(connectionString);
        }
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
