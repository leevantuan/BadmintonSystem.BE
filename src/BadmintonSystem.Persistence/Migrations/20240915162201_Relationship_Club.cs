using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonSystem.Persistence.Migrations;

/// <inheritdoc />
public partial class Relationship_Club : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<Guid>(
            name: "ClubId",
            table: "AdditionalService",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "ClubId1",
            table: "AdditionalService",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_AdditionalService_ClubId",
            table: "AdditionalService",
            column: "ClubId");

        migrationBuilder.CreateIndex(
            name: "IX_AdditionalService_ClubId1",
            table: "AdditionalService",
            column: "ClubId1");

        migrationBuilder.AddForeignKey(
            name: "FK_AdditionalService_Club_ClubId",
            table: "AdditionalService",
            column: "ClubId",
            principalTable: "Club",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

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
            name: "FK_AdditionalService_Club_ClubId",
            table: "AdditionalService");

        migrationBuilder.DropForeignKey(
            name: "FK_AdditionalService_Club_ClubId1",
            table: "AdditionalService");

        migrationBuilder.DropIndex(
            name: "IX_AdditionalService_ClubId",
            table: "AdditionalService");

        migrationBuilder.DropIndex(
            name: "IX_AdditionalService_ClubId1",
            table: "AdditionalService");

        migrationBuilder.DropColumn(
            name: "ClubId1",
            table: "AdditionalService");

        migrationBuilder.AlterColumn<Guid>(
            name: "ClubId",
            table: "AdditionalService",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier");
    }
}
