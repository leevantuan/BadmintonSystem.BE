using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_YardType_Price_PriceId",
                table: "YardType");

            migrationBuilder.DropIndex(
                name: "IX_YardType_PriceId",
                table: "YardType");

            migrationBuilder.DropColumn(
                name: "PriceId",
                table: "YardType");

            migrationBuilder.AddColumn<Guid>(
                name: "YardTypeId",
                table: "Price",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Price_YardTypeId",
                table: "Price",
                column: "YardTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Price_YardType_YardTypeId",
                table: "Price",
                column: "YardTypeId",
                principalTable: "YardType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Price_YardType_YardTypeId",
                table: "Price");

            migrationBuilder.DropIndex(
                name: "IX_Price_YardTypeId",
                table: "Price");

            migrationBuilder.DropColumn(
                name: "YardTypeId",
                table: "Price");

            migrationBuilder.AddColumn<Guid>(
                name: "PriceId",
                table: "YardType",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_YardType_PriceId",
                table: "YardType",
                column: "PriceId");

            migrationBuilder.AddForeignKey(
                name: "FK_YardType_Price_PriceId",
                table: "YardType",
                column: "PriceId",
                principalTable: "Price",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
