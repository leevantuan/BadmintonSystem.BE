using System.Security.Claims;
using BadmintonSystem.Application.Extensions;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.AspNetCore.Identity;
using Action = BadmintonSystem.Domain.Entities.Identity.Action;

namespace BadmintonSystem.Infrastructure.Seed;

public sealed class DbSeeder(
    ApplicationDbContext context,
    RoleManager<AppRole> roleManager,
    UserManager<AppUser> userManager)
    : IDbSeeder
{
    public async Task SeedAsync()
    {
        // entity
        await TimeSlotSeeder();
        await YardTypeSeeder();
        await YardSeeder();
        await PriceSeeder();

        // identity
        await ActionSeeder();
        await AppRoleSeeder();
        await FunctionSeeder();
        await AppUserSeeder();

        // Set role
        await AppRoleClaimWithAdminSeeder();
        await AppRoleClaimWithManagerSeeder();
        await AppRoleClaimWithCustomerSeeder();
        await AppUserClaimsSeeder();
    }

    // Seeder Price
    private async Task PriceSeeder()
    {
        if (!context.Price.Any())
        {
            Guid yardTypeId = context.YardType.FirstOrDefault(x => x.Name == YardTypeEnum.NORMAL.ToString()).Id;

            var priceEntities = new List<Price>();

            foreach (object? dayOfWeek in Enum.GetValues(typeof(DayOfWeekEnum)))
            {
                var priceEntityStart = new Price
                {
                    Id = Guid.NewGuid(),
                    YardTypeId = yardTypeId,
                    YardPrice = 60000,
                    Detail = string.Empty,
                    StartTime = TimeSpan.Parse("00:00:00"),
                    EndTime = TimeSpan.Parse("16:59:59"),
                    DayOfWeek = dayOfWeek.ToString(),
                    IsDefault = DefaultEnum.FALSE
                };

                priceEntities.Add(priceEntityStart);

                var priceEntityEnd = new Price
                {
                    Id = Guid.NewGuid(),
                    YardTypeId = yardTypeId,
                    YardPrice = 80000,
                    Detail = string.Empty,
                    StartTime = TimeSpan.Parse("17:00:00"),
                    EndTime = TimeSpan.Parse("23:59:59"),
                    DayOfWeek = dayOfWeek.ToString(),
                    IsDefault = DefaultEnum.FALSE
                };

                priceEntities.Add(priceEntityEnd);
            }

            context.Price.AddRange(priceEntities);

            await context.SaveChangesAsync();
        }
    }

    // Seeder Yard
    private async Task YardSeeder()
    {
        if (!context.Yard.Any())
        {
            var yardNames = new List<string>
            {
                "A",
                "B",
                "C",
                "D",
                "E"
            };

            Guid yardTypeId = context.YardType.FirstOrDefault(x => x.Name == YardTypeEnum.NORMAL.ToString()).Id;

            var yardEntities = new List<Yard>();

            foreach (string yardName in yardNames)
            {
                var yardEntity = new Yard
                {
                    Id = Guid.NewGuid(),
                    Name = yardName,
                    YardTypeId = yardTypeId,
                    IsStatus = StatusEnum.TRUE
                };

                yardEntities.Add(yardEntity);
            }

            context.Yard.AddRange(yardEntities);

            await context.SaveChangesAsync();
        }
    }

    // Seeder Yard Type
    private async Task YardTypeSeeder()
    {
        if (!context.YardType.Any())
        {
            var yardTypeEntities = new YardType
            {
                Id = Guid.NewGuid(),
                Name = YardTypeEnum.NORMAL.ToString()
            };

            context.YardType.Add(yardTypeEntities);

            await context.SaveChangesAsync();
        }
    }

    // Seeder Time Slot
    private async Task TimeSlotSeeder()
    {
        if (!context.TimeSlot.Any())
        {
            var timeSlotEntities = new List<TimeSlot>();

            foreach (TimeSlotEnum timeSlotEnum in Enum.GetValues(typeof(TimeSlotEnum)))
            {
                (TimeSpan StartTime, TimeSpan EndTime) timeSlot = TimeSlotEnumExtension.GetTimeSlotTimes(timeSlotEnum);
                var timeSlotEntity = new TimeSlot
                {
                    Id = Guid.NewGuid(),
                    StartTime = timeSlot.StartTime,
                    EndTime = timeSlot.EndTime
                };

                timeSlotEntities.Add(timeSlotEntity);
            }

            context.TimeSlot.AddRange(timeSlotEntities);
            await context.SaveChangesAsync();
        }
    }

    // Seeder Action
    private async Task ActionSeeder()
    {
        if (!context.Actions.Any())
        {
            foreach (ActionEnum actionEnum in Enum.GetValues(typeof(ActionEnum)))
            {
                string actionName = actionEnum.ToString();
                int actionValue = (int)actionEnum;

                var newAction = new Action
                {
                    Name = actionName,
                    SortOrder = actionValue,
                    IsActive = true
                };

                context.Actions.Add(newAction);
            }

            await context.SaveChangesAsync();
        }
    }

    // Seeder AppRole
    private async Task AppRoleSeeder()
    {
        if (!roleManager.Roles.Any())
        {
            foreach (AppRoleEnum roleEnum in Enum.GetValues(typeof(AppRoleEnum)))
            {
                string roleName = roleEnum.ToString();

                var role = new AppRole
                {
                    Name = StringExtension.CapitalizeFirstLetter(roleName),
                    RoleCode = roleName,
                    Description = roleName
                };

                await roleManager.CreateAsync(role);
            }

            await context.SaveChangesAsync();
        }
    }

    // Seeder Function
    private async Task FunctionSeeder()
    {
        if (!context.Functions.Any())
        {
            foreach (FunctionEnum functionEnum in Enum.GetValues(typeof(FunctionEnum)))
            {
                string functionName = functionEnum.ToString();

                var newFunction = new Function
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
        }
    }

    // Seeder AppUser
    private async Task AppUserSeeder()
    {
        if (!userManager.Users.Any())
        {
            var newUser = new AppUser
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
            }
            else
            {
                string.Join(", ", createResult.Errors.Select(e => e.Description));
            }
        }
    }

    // Seeder RoleClaims Admin
    private async Task AppRoleClaimWithAdminSeeder()
    {
        AppRole role = await roleManager.FindByNameAsync(AppRoleEnum.ADMIN.ToString())
                       ?? throw new IdentityException.AppUserNotFoundException(
                           AppRoleEnum.ADMIN.ToString());

        IList<Claim> roleClaims = await roleManager.GetClaimsAsync(role);
        if (!roleClaims.Any())
        {
            foreach (FunctionEnum functionEnum in Enum.GetValues(typeof(FunctionEnum)))
            {
                string functionName = functionEnum.ToString().Trim().ToUpper();

                await AddAppRoleClaim(functionName, "63", role);
            }
        }
    }

    // Seeder RoleClaims Manager
    private async Task AppRoleClaimWithManagerSeeder()
    {
        AppRole role = await roleManager.FindByNameAsync(AppRoleEnum.MANAGER.ToString())
                       ?? throw new IdentityException.AppUserNotFoundException(
                           AppRoleEnum.ADMIN.ToString());

        IList<Claim> roleClaims = await roleManager.GetClaimsAsync(role);
        if (!roleClaims.Any())
        {
            foreach (FunctionEnum functionEnum in Enum.GetValues(typeof(FunctionEnum)))
            {
                string functionName = functionEnum.ToString().Trim().ToUpper();

                await AddAppRoleClaim(functionName, "63", role);
            }
        }
    }

    // Seeder RoleClaims Customer
    private async Task AppRoleClaimWithCustomerSeeder()
    {
        AppRole role = await roleManager.FindByNameAsync(AppRoleEnum.CUSTOMER.ToString())
                       ?? throw new IdentityException.AppUserNotFoundException(
                           AppRoleEnum.ADMIN.ToString());

        IList<Claim> roleClaims = await roleManager.GetClaimsAsync(role);

        if (!roleClaims.Any())
        {
            foreach (FunctionEnum functionEnum in Enum.GetValues(typeof(FunctionEnum)))
            {
                string functionName = functionEnum.ToString().Trim().ToUpper();

                if (functionName == FunctionEnum.ADMINISTRATOR.ToString())
                {
                    await AddAppRoleClaim(functionName, "0", role);
                    continue;
                }

                await AddAppRoleClaim(functionName, "63", role);
            }
        }
    }

    // Seeder UserClaims
    private async Task AppUserClaimsSeeder()
    {
        string userEmail = "admin@gmail.com";
        AppUser user = await userManager.FindByEmailAsync(userEmail)
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
                }
            }
        }
    }

    private async Task AddAppRoleClaim(string functionName, string value, AppRole role)
    {
        var claim = new Claim(functionName, value);

        await roleManager.AddClaimAsync(role, claim);
        await context.SaveChangesAsync();
    }
}
