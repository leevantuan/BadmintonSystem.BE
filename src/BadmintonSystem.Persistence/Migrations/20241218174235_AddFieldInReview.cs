using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldInReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClubId",
                table: "Review",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Review_ClubId",
                table: "Review",
                column: "ClubId");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Club_ClubId",
                table: "Review",
                column: "ClubId",
                principalTable: "Club",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_Club_ClubId",
                table: "Review");

            migrationBuilder.DropIndex(
                name: "IX_Review_ClubId",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "ClubId",
                table: "Review");
        }
    }
}
