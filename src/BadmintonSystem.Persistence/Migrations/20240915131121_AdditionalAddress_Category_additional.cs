using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonSystem.Persistence.Migrations;

/// <inheritdoc />
public partial class AdditionalAddress_Category_additional : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Address",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                AddressLine_1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                AddressLine_2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                UserIdCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                UserIdModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeleteFlag = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Address", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Category",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                UserIdCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                UserIdModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeleteFlag = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Category", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AdditionalService",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CategoryId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                UserIdCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                UserIdModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeleteFlag = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AdditionalService", x => x.Id);
                table.ForeignKey(
                    name: "FK_AdditionalService_Category_CategoryId",
                    column: x => x.CategoryId,
                    principalTable: "Category",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_AdditionalService_Category_CategoryId1",
                    column: x => x.CategoryId1,
                    principalTable: "Category",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_AdditionalService_CategoryId",
            table: "AdditionalService",
            column: "CategoryId");

        migrationBuilder.CreateIndex(
            name: "IX_AdditionalService_CategoryId1",
            table: "AdditionalService",
            column: "CategoryId1");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AdditionalService");

        migrationBuilder.DropTable(
            name: "Address");

        migrationBuilder.DropTable(
            name: "Category");
    }
}
