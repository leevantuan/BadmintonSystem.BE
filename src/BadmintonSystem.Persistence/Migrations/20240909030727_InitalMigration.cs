﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonSystem.Persistence.Migrations;

/// <inheritdoc />
public partial class InitalMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Actions",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                SortOrder = table.Column<int>(type: "int", nullable: true),
                IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Actions", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AppRoles",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                RoleCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AppRoles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AppUsers",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                DayOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                IsDirector = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                IsHeadOfDepartment = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                ManagerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PositionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                IsRecipient = table.Column<int>(type: "int", nullable: true, defaultValue: -1),
                UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                AccessFailedCount = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AppUsers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Functions",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Url = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                ParrentId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                SortOrder = table.Column<int>(type: "int", nullable: true),
                CssClass = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Functions", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Gender",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                UserIdCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                UserIdModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeleteFlag = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Gender", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AppRoleClaims",
            columns: table => new
            {
                RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Id = table.Column<int>(type: "int", nullable: false),
                ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AppRoleClaims", x => x.RoleId);
                table.ForeignKey(
                    name: "FK_AppRoleClaims_AppRoles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "AppRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AppUserClaims",
            columns: table => new
            {
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Id = table.Column<int>(type: "int", nullable: false),
                ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AppUserClaims", x => x.UserId);
                table.ForeignKey(
                    name: "FK_AppUserClaims_AppUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AppUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AppUserLogins",
            columns: table => new
            {
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                LoginProvider = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ProviderKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AppUserLogins", x => x.UserId);
                table.ForeignKey(
                    name: "FK_AppUserLogins_AppUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AppUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AppUserRoles",
            columns: table => new
            {
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AppUserRoles", x => new { x.RoleId, x.UserId });
                table.ForeignKey(
                    name: "FK_AppUserRoles_AppRoles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "AppRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_AppUserRoles_AppUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AppUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AppUserTokens",
            columns: table => new
            {
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                LoginProvider = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AppUserTokens", x => x.UserId);
                table.ForeignKey(
                    name: "FK_AppUserTokens_AppUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AppUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ActionInFunctions",
            columns: table => new
            {
                ActionId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                FunctionId = table.Column<string>(type: "nvarchar(50)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ActionInFunctions", x => new { x.ActionId, x.FunctionId });
                table.ForeignKey(
                    name: "FK_ActionInFunctions_Actions_ActionId",
                    column: x => x.ActionId,
                    principalTable: "Actions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_ActionInFunctions_Functions_FunctionId",
                    column: x => x.FunctionId,
                    principalTable: "Functions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "PermissionInRoles",
            columns: table => new
            {
                RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FunctionId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                BinaryValue = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PermissionInRoles", x => new { x.RoleId, x.FunctionId });
                table.ForeignKey(
                    name: "FK_PermissionInRoles_AppRoles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "AppRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_PermissionInRoles_Functions_FunctionId",
                    column: x => x.FunctionId,
                    principalTable: "Functions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "PermissionInUsers",
            columns: table => new
            {
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FunctionId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                BinaryValue = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PermissionInUsers", x => new { x.UserId, x.FunctionId });
                table.ForeignKey(
                    name: "FK_PermissionInUsers_AppUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AppUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_PermissionInUsers_Functions_FunctionId",
                    column: x => x.FunctionId,
                    principalTable: "Functions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ActionInFunctions_FunctionId",
            table: "ActionInFunctions",
            column: "FunctionId");

        migrationBuilder.CreateIndex(
            name: "IX_AppUserRoles_UserId",
            table: "AppUserRoles",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_PermissionInRoles_FunctionId",
            table: "PermissionInRoles",
            column: "FunctionId");

        migrationBuilder.CreateIndex(
            name: "IX_PermissionInUsers_FunctionId",
            table: "PermissionInUsers",
            column: "FunctionId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ActionInFunctions");

        migrationBuilder.DropTable(
            name: "AppRoleClaims");

        migrationBuilder.DropTable(
            name: "AppUserClaims");

        migrationBuilder.DropTable(
            name: "AppUserLogins");

        migrationBuilder.DropTable(
            name: "AppUserRoles");

        migrationBuilder.DropTable(
            name: "AppUserTokens");

        migrationBuilder.DropTable(
            name: "Gender");

        migrationBuilder.DropTable(
            name: "PermissionInRoles");

        migrationBuilder.DropTable(
            name: "PermissionInUsers");

        migrationBuilder.DropTable(
            name: "Actions");

        migrationBuilder.DropTable(
            name: "AppRoles");

        migrationBuilder.DropTable(
            name: "AppUsers");

        migrationBuilder.DropTable(
            name: "Functions");
    }
}
