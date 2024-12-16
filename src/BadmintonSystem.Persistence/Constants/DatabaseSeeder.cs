using System.Security.Claims;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Entities = BadmintonSystem.Domain.Entities.Identity;

namespace BadmintonSystem.Persistence.Constants;

public static class DatabaseSeeder
{
    // Seeder Action
    public static async Task ActionSeeder(ApplicationDbContext context, ILogger logger)
    {
        if (!context.Actions.Any())
        {
            foreach (ActionEnum actionEnum in Enum.GetValues(typeof(ActionEnum)))
            {
                string actionName = actionEnum.ToString();
                int actionValue = (int)actionEnum;

                var newAction = new Entities.Action
                {
                    Name = actionName,
                    SortOrder = actionValue,
                    IsActive = true
                };

                context.Actions.Add(newAction);
            }

            await context.SaveChangesAsync();
            logger.LogInformation("Seeded default data for Action.");
        }
    }

    // Seeder AppRole
    public static async Task AppRoleSeeder
    (RoleManager<Entities.AppRole> roleManager, ApplicationDbContext context,
        ILogger logger)
    {
        if (!roleManager.Roles.Any())
        {
            foreach (AppRoleEnum roleEnum in Enum.GetValues(typeof(AppRoleEnum)))
            {
                string roleName = roleEnum.ToString();

                var role = new Entities.AppRole
                {
                    Name = StringExtension.CapitalizeFirstLetter(roleName),
                    RoleCode = roleName,
                    Description = roleName
                };

                await roleManager.CreateAsync(role);
            }

            await context.SaveChangesAsync();
            logger.LogInformation("Seeded default data for Role.");
        }
    }

    // Seeder Function
    public static async Task FunctionSeeder(ApplicationDbContext context, ILogger logger)
    {
        if (!context.Functions.Any())
        {
            foreach (FunctionEnum functionEnum in Enum.GetValues(typeof(FunctionEnum)))
            {
                string functionName = functionEnum.ToString();

                var newFunction = new Entities.Function
                {
                    Name = functionName,
                    Status = FunctionStatus.Active,
                    Key = functionName.ToUpper(),
                    ActionValue = 63,
                    SortOrder = (int)functionEnum,
                    Url = "/appuser" // Cung cấp giá trị cho cột Url
                };

                context.Functions.Add(newFunction);
            }

            await context.SaveChangesAsync();
            logger.LogInformation("Seeded default data for Function.");
        }
    }

    // Seeder AppUser
    public static async Task AppUserSeeder
    (UserManager<Entities.AppUser> userManager, ApplicationDbContext context,
        ILogger logger)
    {
        if (!userManager.Users.Any())
        {
            var newUser = new Entities.AppUser
            {
                UserName = "Admin",
                Email = "admin@gmail.com",
                FirstName = "Admin",
                LastName = "Admin",
                FullName = "Admin",
                SecurityStamp = Guid.NewGuid().ToString() // Set a unique security stamp
            };

            // Tạo và lưu người dùng trước
            IdentityResult createResult = await userManager.CreateAsync(newUser, "123456@Aa");

            if (createResult.Succeeded)
            {
                string roleName = StringExtension.CapitalizeFirstLetter(AppRoleEnum.ADMIN.ToString());

                // Sau đó, thêm vai trò cho người dùng
                await userManager.AddToRoleAsync(newUser, roleName);

                await context.SaveChangesAsync(); // Lưu các thay đổi vào cơ sở dữ liệu
                logger.LogInformation("Seeded default data for user.");
            }
            else
            {
                logger.LogError("Failed to create user: {Errors}",
                    string.Join(", ", createResult.Errors.Select(e => e.Description)));
            }
        }
    }

    // Seeder RoleClaims Admin
    public static async Task AppRoleClaimWithAdminSeeder
    (RoleManager<Entities.AppRole> roleManager,
        ApplicationDbContext context)
    {
        Entities.AppRole role = await roleManager.FindByNameAsync(AppRoleEnum.ADMIN.ToString())
                                ?? throw new IdentityException.AppUserNotFoundException(AppRoleEnum.ADMIN.ToString());

        IList<Claim> roleClaims = await roleManager.GetClaimsAsync(role);
        if (!roleClaims.Any())
        {
            foreach (FunctionEnum functionEnum in Enum.GetValues(typeof(FunctionEnum)))
            {
                string functionName = functionEnum.ToString().Trim().ToUpper();

                await AddAppRoleClaim(functionName, "63", role, context, roleManager);
            }
        }
    }

    // Seeder RoleClaims Manager
    public static async Task AppRoleClaimWithManagerSeeder
    (RoleManager<Entities.AppRole> roleManager,
        ApplicationDbContext context)
    {
        Entities.AppRole role = await roleManager.FindByNameAsync(AppRoleEnum.MANAGER.ToString())
                                ?? throw new IdentityException.AppUserNotFoundException(AppRoleEnum.ADMIN.ToString());

        IList<Claim> roleClaims = await roleManager.GetClaimsAsync(role);
        if (!roleClaims.Any())
        {
            foreach (FunctionEnum functionEnum in Enum.GetValues(typeof(FunctionEnum)))
            {
                string functionName = functionEnum.ToString().Trim().ToUpper();

                await AddAppRoleClaim(functionName, "63", role, context, roleManager);
            }
        }
    }

    // Seeder RoleClaims Customer
    public static async Task AppRoleClaimWithCustomerSeeder
    (RoleManager<Entities.AppRole> roleManager,
        ApplicationDbContext context)
    {
        Entities.AppRole role = await roleManager.FindByNameAsync(AppRoleEnum.CUSTOMER.ToString())
                                ?? throw new IdentityException.AppUserNotFoundException(AppRoleEnum.ADMIN.ToString());

        IList<Claim> roleClaims = await roleManager.GetClaimsAsync(role);

        if (!roleClaims.Any())
        {
            foreach (FunctionEnum functionEnum in Enum.GetValues(typeof(FunctionEnum)))
            {
                string functionName = functionEnum.ToString().Trim().ToUpper();
                switch (functionName)
                {
                    case "ADMINISTRATOR":
                        await AddAppRoleClaim(functionName, "0", role, context, roleManager);
                        break;
                    case "ADDRESS":
                        await AddAppRoleClaim(functionName, "15", role, context, roleManager);
                        break;
                    case "APPUSER":
                        await AddAppRoleClaim(functionName, "15", role, context, roleManager);
                        break;
                    case "BOOKING":
                        await AddAppRoleClaim(functionName, "15", role, context, roleManager);
                        break;
                    case "CATEGORY":
                        await AddAppRoleClaim(functionName, "15", role, context, roleManager);
                        break;
                    case "CLUB":
                        await AddAppRoleClaim(functionName, "15", role, context, roleManager);
                        break;
                    case "NOFITICATION":
                        await AddAppRoleClaim(functionName, "15", role, context, roleManager);
                        break;
                    case "REVIEW":
                        await AddAppRoleClaim(functionName, "15", role, context, roleManager);
                        break;
                    case "SALE":
                        await AddAppRoleClaim(functionName, "15", role, context, roleManager);
                        break;
                    case "SERVICE":
                        await AddAppRoleClaim(functionName, "15", role, context, roleManager);
                        break;
                    case "TIMESLOT":
                        await AddAppRoleClaim(functionName, "15", role, context, roleManager);
                        break;
                    case "YARD":
                        await AddAppRoleClaim(functionName, "15", role, context, roleManager);
                        break;
                    case "YARDTYPE":
                        await AddAppRoleClaim(functionName, "15", role, context, roleManager);
                        break;
                }
            }
        }
    }

    // Seeder UserClaims
    public static async Task AppUserClaimsSeeder
    (UserManager<Entities.AppUser> userManager,
        ApplicationDbContext context, ILogger logger)
    {
        string userEmail = "admin@gmail.com";
        Entities.AppUser user = await userManager.FindByEmailAsync(userEmail)
                                ?? throw new IdentityException.AppUserNotFoundException(userEmail);

        IList<Claim> userClaims = await userManager.GetClaimsAsync(user);
        if (!userClaims.Any())
        {
            foreach (FunctionEnum functionEnum in Enum.GetValues(typeof(FunctionEnum)))
            {
                string functionName = functionEnum.ToString().Trim().ToUpper();
                var claim = new Claim(functionName, "63");

                // Lấy danh sách claims hiện có của người dùng
                IList<Claim> existingClaims = await userManager.GetClaimsAsync(user);

                // Kiểm tra xem claim đã tồn tại chưa
                if (!existingClaims.Any(c => c.Type == claim.Type && c.Value == claim.Value))
                {
                    await userManager.AddClaimAsync(user, claim);
                    await context.SaveChangesAsync();
                    logger.LogInformation("Seeded default data for User Claims.");
                }
                else
                {
                    logger.LogInformation("Claim already exists for user {UserEmail}.", userEmail);
                }
            }
        }
    }

    private static async Task AddAppRoleClaim
    (string functionName, string value,
        Entities.AppRole role, ApplicationDbContext context, RoleManager<Entities.AppRole> roleManager)
    {
        var claim = new Claim(functionName, value);

        await roleManager.AddClaimAsync(role, claim);
        await context.SaveChangesAsync();
    }
}
