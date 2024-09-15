using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonSystem.Persistence.Migrations;

/// <inheritdoc />
public partial class Add_Club_UserAddress : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "ClubId",
            table: "Address",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.AddColumn<Guid>(
            name: "ClubId1",
            table: "Address",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "Club",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
                Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                HotLine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                FacebookPageLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                InstagramPageLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                MapLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ImageLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                OpeningTime = table.Column<TimeSpan>(type: "time", nullable: true),
                ClosingTime = table.Column<TimeSpan>(type: "time", nullable: true),
                DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                UserIdCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                UserIdModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeleteFlag = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Club", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "UserAddress",
            columns: table => new
            {
                AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                IsDefault = table.Column<bool>(type: "bit", nullable: false),
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                UserIdCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                UserIdModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeleteFlag = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserAddress", x => new { x.AddressId, x.AppUserId });
                table.ForeignKey(
                    name: "FK_UserAddress_Address_AddressId",
                    column: x => x.AddressId,
                    principalTable: "Address",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_UserAddress_AppUsers_AppUserId",
                    column: x => x.AppUserId,
                    principalTable: "AppUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Address_ClubId",
            table: "Address",
            column: "ClubId");

        migrationBuilder.CreateIndex(
            name: "IX_Address_ClubId1",
            table: "Address",
            column: "ClubId1");

        migrationBuilder.CreateIndex(
            name: "IX_UserAddress_AppUserId",
            table: "UserAddress",
            column: "AppUserId");

        migrationBuilder.AddForeignKey(
            name: "FK_Address_Club_ClubId",
            table: "Address",
            column: "ClubId",
            principalTable: "Club",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Address_Club_ClubId1",
            table: "Address",
            column: "ClubId1",
            principalTable: "Club",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Address_Club_ClubId",
            table: "Address");

        migrationBuilder.DropForeignKey(
            name: "FK_Address_Club_ClubId1",
            table: "Address");

        migrationBuilder.DropTable(
            name: "Club");

        migrationBuilder.DropTable(
            name: "UserAddress");

        migrationBuilder.DropIndex(
            name: "IX_Address_ClubId",
            table: "Address");

        migrationBuilder.DropIndex(
            name: "IX_Address_ClubId1",
            table: "Address");

        migrationBuilder.DropColumn(
            name: "ClubId",
            table: "Address");

        migrationBuilder.DropColumn(
            name: "ClubId1",
            table: "Address");
    }
}
