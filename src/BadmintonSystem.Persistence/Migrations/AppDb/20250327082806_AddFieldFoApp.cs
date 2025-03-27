using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonSystem.Persistence.Migrations.AppDb;

/// <inheritdoc />
public partial class AddFieldFoApp : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Description",
            table: "Service",
            type: "text",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "ImageLink",
            table: "Service",
            type: "text",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Description",
            table: "Club",
            type: "text",
            nullable: false,
            defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Description",
            table: "Service");

        migrationBuilder.DropColumn(
            name: "ImageLink",
            table: "Service");

        migrationBuilder.DropColumn(
            name: "Description",
            table: "Club");
    }
}
