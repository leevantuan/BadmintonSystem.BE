using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonSystem.Persistence.Migrations;

/// <inheritdoc />
public partial class Test : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "ClubId1",
            table: "AdditionalService",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_AdditionalService_ClubId1",
            table: "AdditionalService",
            column: "ClubId1");

        migrationBuilder.AddForeignKey(
            name: "FK_AdditionalService_Club_ClubId1",
            table: "AdditionalService",
            column: "ClubId1",
            principalTable: "Club",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_AdditionalService_Club_ClubId1",
            table: "AdditionalService");

        migrationBuilder.DropIndex(
            name: "IX_AdditionalService_ClubId1",
            table: "AdditionalService");

        migrationBuilder.DropColumn(
            name: "ClubId1",
            table: "AdditionalService");
    }
}
