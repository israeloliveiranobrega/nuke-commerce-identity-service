using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NukeAuthentication.Migrations
{
    /// <inheritdoc />
    public partial class testCommit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Is_revoked",
                table: "user_session",
                newName: "is_revoked");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_revoked",
                table: "user_session",
                newName: "Is_revoked");
        }
    }
}
