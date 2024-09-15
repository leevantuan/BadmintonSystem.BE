using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonSystem.Persistence.Migrations;

/// <inheritdoc />
public partial class Change_Relationship : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_AdditionalService_Category_CategoryId1",
            table: "AdditionalService");

        migrationBuilder.DropForeignKey(
            name: "FK_AdditionalService_Club_ClubId1",
            table: "AdditionalService");

        migrationBuilder.DropForeignKey(
            name: "FK_Address_Club_ClubId1",
            table: "Address");

        migrationBuilder.DropIndex(
            name: "IX_Address_ClubId1",
            table: "Address");

        migrationBuilder.DropIndex(
            name: "IX_AdditionalService_CategoryId1",
            table: "AdditionalService");

        migrationBuilder.DropIndex(
            name: "IX_AdditionalService_ClubId1",
            table: "AdditionalService");

        migrationBuilder.DropColumn(
            name: "ClubId1",
            table: "Address");

        migrationBuilder.DropColumn(
            name: "CategoryId1",
            table: "AdditionalService");

        migrationBuilder.DropColumn(
            name: "ClubId1",
            table: "AdditionalService");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "ClubId1",
            table: "Address",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "CategoryId1",
            table: "AdditionalService",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "ClubId1",
            table: "AdditionalService",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Address_ClubId1",
            table: "Address",
            column: "ClubId1");

        migrationBuilder.CreateIndex(
            name: "IX_AdditionalService_CategoryId1",
            table: "AdditionalService",
            column: "CategoryId1");

        migrationBuilder.CreateIndex(
            name: "IX_AdditionalService_ClubId1",
            table: "AdditionalService",
            column: "ClubId1");

        migrationBuilder.AddForeignKey(
            name: "FK_AdditionalService_Category_CategoryId1",
            table: "AdditionalService",
            column: "CategoryId1",
            principalTable: "Category",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_AdditionalService_Club_ClubId1",
            table: "AdditionalService",
            column: "ClubId1",
            principalTable: "Club",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Address_Club_ClubId1",
            table: "Address",
            column: "ClubId1",
            principalTable: "Club",
            principalColumn: "Id");
    }
}
