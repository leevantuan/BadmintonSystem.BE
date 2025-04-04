using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonSystem.Persistence.Migrations.TenantDb;

/// <inheritdoc />
public partial class AddTableHstory : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "BookingHistories",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                ClubName = table.Column<string>(type: "text", nullable: false),
                StartTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                PlayDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                TotalPrice = table.Column<decimal>(type: "numeric", nullable: false),
                PaymentStatus = table.Column<int>(type: "integer", nullable: false),
                TenantCode = table.Column<string>(type: "text", nullable: false),
                UserId = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_BookingHistories", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "BookingHistories");
    }
}
