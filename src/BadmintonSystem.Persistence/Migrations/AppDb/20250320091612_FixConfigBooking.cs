using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonSystem.Persistence.Migrations.AppDb;

/// <inheritdoc />
public partial class FixConfigBooking : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Booking_AppUsers_UserId",
            table: "Booking");

        migrationBuilder.DropForeignKey(
            name: "FK_ChatMessage_ChatRoom_ChatRoomId",
            table: "ChatMessage");

        migrationBuilder.DropIndex(
            name: "IX_Booking_UserId",
            table: "Booking");

        migrationBuilder.AlterColumn<Guid>(
            name: "UserId",
            table: "Booking",
            type: "uuid",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uuid");

        migrationBuilder.AddColumn<Guid>(
            name: "AppUserId",
            table: "Booking",
            type: "uuid",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Booking_AppUserId",
            table: "Booking",
            column: "AppUserId");

        migrationBuilder.AddForeignKey(
            name: "FK_Booking_AppUsers_AppUserId",
            table: "Booking",
            column: "AppUserId",
            principalTable: "AppUsers",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_ChatMessage_ChatRoom_ChatRoomId",
            table: "ChatMessage",
            column: "ChatRoomId",
            principalTable: "ChatRoom",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Booking_AppUsers_AppUserId",
            table: "Booking");

        migrationBuilder.DropForeignKey(
            name: "FK_ChatMessage_ChatRoom_ChatRoomId",
            table: "ChatMessage");

        migrationBuilder.DropIndex(
            name: "IX_Booking_AppUserId",
            table: "Booking");

        migrationBuilder.DropColumn(
            name: "AppUserId",
            table: "Booking");

        migrationBuilder.AlterColumn<Guid>(
            name: "UserId",
            table: "Booking",
            type: "uuid",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldNullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Booking_UserId",
            table: "Booking",
            column: "UserId");

        migrationBuilder.AddForeignKey(
            name: "FK_Booking_AppUsers_UserId",
            table: "Booking",
            column: "UserId",
            principalTable: "AppUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_ChatMessage_ChatRoom_ChatRoomId",
            table: "ChatMessage",
            column: "ChatRoomId",
            principalTable: "ChatRoom",
            principalColumn: "Id");
    }
}
