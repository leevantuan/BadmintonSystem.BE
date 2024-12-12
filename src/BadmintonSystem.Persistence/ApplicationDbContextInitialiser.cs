using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Persistence.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BadmintonSystem.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;

    public ApplicationDbContextInitialiser(
        ILogger<ApplicationDbContextInitialiser> logger,
        ApplicationDbContext context,
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        // Seed dữ liệu mặc định cho các bảng
        await DatabaseSeeder.ActionSeeder(_context, _logger);
        await DatabaseSeeder.AppRoleSeeder(_roleManager, _context, _logger);
        await DatabaseSeeder.AppUserSeeder(_userManager, _context, _logger);
        await DatabaseSeeder.FunctionSeeder(_context, _logger);
        await DatabaseSeeder.AppRoleClaimWithCustomerSeeder(_roleManager, _context);
        await DatabaseSeeder.AppRoleClaimWithManagerSeeder(_roleManager, _context);
        await DatabaseSeeder.AppRoleClaimWithAdminSeeder(_roleManager, _context);
        await DatabaseSeeder.AppUserClaimsSeeder(_userManager, _context, _logger);
    }
}
