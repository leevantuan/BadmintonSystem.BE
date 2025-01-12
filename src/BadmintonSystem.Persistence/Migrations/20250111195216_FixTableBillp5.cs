using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixTableBillp5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bill_Booking_BookingId",
                table: "Bill");

            migrationBuilder.DropForeignKey(
                name: "FK_Bill_Booking_BookingId1",
                table: "Bill");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Sale_SaleId",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Bill_BookingId1",
                table: "Bill");

            migrationBuilder.DropColumn(
                name: "BookingId1",
                table: "Bill");

            migrationBuilder.AddForeignKey(
                name: "FK_Bill_Booking_BookingId",
                table: "Bill",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Sale_SaleId",
                table: "Booking",
                column: "SaleId",
                principalTable: "Sale",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bill_Booking_BookingId",
                table: "Bill");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Sale_SaleId",
                table: "Booking");

            migrationBuilder.AddColumn<Guid>(
                name: "BookingId1",
                table: "Bill",
                type: "uuid",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Sale_SaleId",
                table: "Booking",
                column: "SaleId",
                principalTable: "Sale",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
