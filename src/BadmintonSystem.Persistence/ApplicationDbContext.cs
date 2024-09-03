using BadmintonSystem.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Action = BadmintonSystem.Domain.Entities.Identity.Action;

namespace BadmintonSystem.Persistence;
public sealed class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Add Configuration DBSet<> From Assembly
    // ApplyConfigurationFromAssemby => It Apply for all configurations in Assembly
    // If AssemblyReference.Assembly == It will take all References project here == Persistence.AssemblyReference.Assembly
    // Trick sort create Func == override OnModel...
    // I don't understantd here ... I can use { Return A }, It can also be use by => A
    // Configuration here ... ==>
    protected override void OnModelCreating(ModelBuilder builder) =>
        builder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);

    // Phím tắt override Save ... cái cuối
    // Save change and auto generate date
    //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    //{
    //    foreach (var entry in base.ChangeTracker.Entries<AuditableEntity<Guid>>()
    //        .Where(q => q.State == EntityState.Added || q.State == EntityState.Modified))
    //    {
    //        entry.Entity.DateModified = DateTime.Now;

    //        if (entry.State == EntityState.Added)
    //        {
    //            entry.Entity.DateCreated = DateTime.Now;
    //        }
    //    }

    //    return base.SaveChangesAsync(cancellationToken);
    //}

    // Generate Database => SQL Server
    // Authorization => Xác thực quyền người dùng có quyền truy cập vào hay không
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<AppRole> AppRoles { get; set; }
    public DbSet<Action> Actions { get; set; }
    public DbSet<Function> Functions { get; set; }
    public DbSet<ActionInFunction> ActionInFunctions { get; set; }
    public DbSet<Permission> Permissions { get; set; }
}
