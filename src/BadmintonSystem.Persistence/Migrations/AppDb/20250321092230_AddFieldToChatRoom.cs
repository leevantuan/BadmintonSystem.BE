using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonSystem.Persistence.Migrations.AppDb;

/// <inheritdoc />
public partial class AddFieldToChatRoom : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ChatRoom_AppUsers_UserId",
            table: "ChatRoom");

        migrationBuilder.DropIndex(
            name: "IX_ChatRoom_UserId",
            table: "ChatRoom");

        migrationBuilder.AddColumn<string>(
            name: "Avatar",
            table: "ChatRoom",
            type: "text",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "Email",
            table: "ChatRoom",
            type: "text",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "UserName",
            table: "ChatRoom",
            type: "text",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<Guid>(
            name: "ChatRoomId",
            table: "AppUsers",
            type: "uuid",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_AppUsers_ChatRoomId",
            table: "AppUsers",
            column: "ChatRoomId");

        migrationBuilder.AddForeignKey(
            name: "FK_AppUsers_ChatRoom_ChatRoomId",
            table: "AppUsers",
            column: "ChatRoomId",
            principalTable: "ChatRoom",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_AppUsers_ChatRoom_ChatRoomId",
            table: "AppUsers");

        migrationBuilder.DropIndex(
            name: "IX_AppUsers_ChatRoomId",
            table: "AppUsers");

        migrationBuilder.DropColumn(
            name: "Avatar",
            table: "ChatRoom");

        migrationBuilder.DropColumn(
            name: "Email",
            table: "ChatRoom");

        migrationBuilder.DropColumn(
            name: "UserName",
            table: "ChatRoom");

        migrationBuilder.DropColumn(
            name: "ChatRoomId",
            table: "AppUsers");

        migrationBuilder.CreateIndex(
            name: "IX_ChatRoom_UserId",
            table: "ChatRoom",
            column: "UserId",
            unique: true);

        migrationBuilder.AddForeignKey(
            name: "FK_ChatRoom_AppUsers_UserId",
            table: "ChatRoom",
            column: "UserId",
            principalTable: "AppUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
