using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTableOriginal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OriginalQuantityId",
                table: "Service",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "QuantityPrinciple",
                table: "Service",
                type: "numeric",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OriginalQuantity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalQuantity = table.Column<decimal>(type: "numeric", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OriginalQuantity", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Service_OriginalQuantityId",
                table: "Service",
                column: "OriginalQuantityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Service_OriginalQuantity_OriginalQuantityId",
                table: "Service",
                column: "OriginalQuantityId",
                principalTable: "OriginalQuantity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Service_OriginalQuantity_OriginalQuantityId",
                table: "Service");

            migrationBuilder.DropTable(
                name: "OriginalQuantity");

            migrationBuilder.DropIndex(
                name: "IX_Service_OriginalQuantityId",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "OriginalQuantityId",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "QuantityPrinciple",
                table: "Service");
        }
    }
}
