using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Whispr.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_Messages_ColumnName_SenderId_To_UserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_messages_users_sender_id",
                table: "messages");

            migrationBuilder.RenameColumn(
                name: "sender_id",
                table: "messages",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_messages_sender_id",
                table: "messages",
                newName: "IX_messages_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_messages_users_user_id",
                table: "messages",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_messages_users_user_id",
                table: "messages");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "messages",
                newName: "sender_id");

            migrationBuilder.RenameIndex(
                name: "IX_messages_user_id",
                table: "messages",
                newName: "IX_messages_sender_id");

            migrationBuilder.AddForeignKey(
                name: "FK_messages_users_sender_id",
                table: "messages",
                column: "sender_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
