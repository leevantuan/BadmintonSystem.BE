using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldServiceLineActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Bill");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Bill");

            migrationBuilder.AddColumn<int>(
                name: "IsActive",
                table: "BillLine",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "BillLine");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                table: "Bill",
                type: "interval",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                table: "Bill",
                type: "interval",
                nullable: true);
        }
    }
}
