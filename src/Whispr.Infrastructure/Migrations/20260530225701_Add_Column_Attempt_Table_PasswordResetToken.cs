using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Whispr.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Column_Attempt_Table_PasswordResetToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Attempt",
                table: "password_reset_tokens",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attempt",
                table: "password_reset_tokens");
        }
    }
}
