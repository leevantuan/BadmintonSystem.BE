using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldPercentPrePay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PercentPrePay",
                table: "Booking",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PercentPrePay",
                table: "Booking");
        }
    }
}
