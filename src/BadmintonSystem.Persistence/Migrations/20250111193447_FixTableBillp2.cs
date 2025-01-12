using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixTableBillp2 : Migration
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

            migrationBuilder.DropColumn(
                name: "BillId",
                table: "Booking");

            migrationBuilder.AddColumn<Guid>(
                name: "BookingId1",
                table: "Bill",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bill_BookingId",
                table: "Bill",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bill_BookingId1",
                table: "Bill",
                column: "BookingId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Bill_Booking_BookingId",
                table: "Bill",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bill_Booking_BookingId1",
                table: "Bill",
                column: "BookingId1",
                principalTable: "Booking",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bill_Booking_BookingId",
                table: "Bill");

            migrationBuilder.DropForeignKey(
                name: "FK_Bill_Booking_BookingId1",
                table: "Bill");

            migrationBuilder.DropIndex(
                name: "IX_Bill_BookingId",
                table: "Bill");

            migrationBuilder.DropIndex(
                name: "IX_Bill_BookingId1",
                table: "Bill");

            migrationBuilder.DropColumn(
                name: "BookingId1",
                table: "Bill");

            migrationBuilder.AddColumn<Guid>(
                name: "BillId",
                table: "Booking",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
