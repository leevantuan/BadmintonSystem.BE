using BadmintonSystem.Domain.Entities;
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

    // Generate Database => SQL Server
    // Authorization => Xác thực quyền người dùng có quyền truy cập vào hay không
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<AppRole> AppRoles { get; set; }
    public DbSet<Action> Actions { get; set; }
    public DbSet<Function> Functions { get; set; }
    public DbSet<ActionInFunction> ActionInFunctions { get; set; }
    public DbSet<Permission> Permissions { get; set; }

    public DbSet<Gender> Genders { get; set; }
}
