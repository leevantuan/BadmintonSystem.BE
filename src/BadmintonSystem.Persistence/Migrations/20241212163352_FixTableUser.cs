using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixTableUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_Gender_GenderId",
                table: "AppUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_Gender_GenderId1",
                table: "AppUsers");

            migrationBuilder.DropTable(
                name: "Gender");

            migrationBuilder.DropIndex(
                name: "IX_AppUsers_GenderId",
                table: "AppUsers");

            migrationBuilder.DropIndex(
                name: "IX_AppUsers_GenderId1",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "GenderId",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "GenderId1",
                table: "AppUsers");

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "AppUsers",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AppUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "GenderId",
                table: "AppUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GenderId1",
                table: "AppUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Gender",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gender", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_GenderId",
                table: "AppUsers",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_GenderId1",
                table: "AppUsers",
                column: "GenderId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_Gender_GenderId",
                table: "AppUsers",
                column: "GenderId",
                principalTable: "Gender",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_Gender_GenderId1",
                table: "AppUsers",
                column: "GenderId1",
                principalTable: "Gender",
                principalColumn: "Id");
        }
    }
}
