using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Whispr.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Table_MessageAttachments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "message_attachments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    storage_key = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    file_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    content_type = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    size_bytes = table.Column<long>(type: "bigint", maxLength: 1024, nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_message_attachments", x => x.id);
                    table.ForeignKey(
                        name: "FK_message_attachments_messages_message_id",
                        column: x => x.message_id,
                        principalTable: "messages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_message_attachments_created_at_utc",
                table: "message_attachments",
                column: "created_at_utc");

            migrationBuilder.CreateIndex(
                name: "IX_message_attachments_message_id",
                table: "message_attachments",
                column: "message_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "message_attachments");
        }
    }
}
