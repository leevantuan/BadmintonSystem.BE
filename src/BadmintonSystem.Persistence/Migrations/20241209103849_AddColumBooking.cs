using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddColumBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMethod_PaymentType_PaymentTypeId",
                table: "PaymentMethod");

            migrationBuilder.AlterColumn<Guid>(
                name: "PaymentTypeId",
                table: "PaymentMethod",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<int>(
                name: "BookingStatus",
                table: "Booking",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatus",
                table: "Booking",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMethod_PaymentType_PaymentTypeId",
                table: "PaymentMethod",
                column: "PaymentTypeId",
                principalTable: "PaymentType",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMethod_PaymentType_PaymentTypeId",
                table: "PaymentMethod");

            migrationBuilder.DropColumn(
                name: "BookingStatus",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Booking");

            migrationBuilder.AlterColumn<Guid>(
                name: "PaymentTypeId",
                table: "PaymentMethod",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMethod_PaymentType_PaymentTypeId",
                table: "PaymentMethod",
                column: "PaymentTypeId",
                principalTable: "PaymentType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
