using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonSystem.Persistence.Migrations.AppDb;

/// <inheritdoc />
public partial class AddRelationShipForChatMess : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ChatMessage_ChatRoom_ChatRoomId",
            table: "ChatMessage");

        migrationBuilder.AddForeignKey(
            name: "FK_ChatMessage_ChatRoom_ChatRoomId",
            table: "ChatMessage",
            column: "ChatRoomId",
            principalTable: "ChatRoom",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ChatMessage_ChatRoom_ChatRoomId",
            table: "ChatMessage");

        migrationBuilder.AddForeignKey(
            name: "FK_ChatMessage_ChatRoom_ChatRoomId",
            table: "ChatMessage",
            column: "ChatRoomId",
            principalTable: "ChatRoom",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
