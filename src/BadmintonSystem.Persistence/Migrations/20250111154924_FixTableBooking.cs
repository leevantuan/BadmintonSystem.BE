using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixTableBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Bill_BillId",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_BillId",
                table: "Booking");

            migrationBuilder.CreateIndex(
                name: "IX_Bill_BookingId",
                table: "Bill",
                column: "BookingId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bill_Booking_BookingId",
                table: "Bill",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bill_Booking_BookingId",
                table: "Bill");

            migrationBuilder.DropIndex(
                name: "IX_Bill_BookingId",
                table: "Bill");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_BillId",
                table: "Booking",
                column: "BillId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Bill_BillId",
                table: "Booking",
                column: "BillId",
                principalTable: "Bill",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
