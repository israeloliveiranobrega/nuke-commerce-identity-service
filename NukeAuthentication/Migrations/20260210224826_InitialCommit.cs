using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NukeAuthentication.Migrations
{
    /// <inheritdoc />
    public partial class InitialCommit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    last_name = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: false),
                    birth_date = table.Column<DateOnly>(type: "date", nullable: false),
                    cpf = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    address_zip_code = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    address_region = table.Column<string>(type: "text", nullable: false),
                    address_state = table.Column<string>(type: "text", nullable: false),
                    address_city = table.Column<string>(type: "text", nullable: false),
                    address_neighborhood = table.Column<string>(type: "text", nullable: false),
                    address_street = table.Column<string>(type: "text", nullable: false),
                    address_number = table.Column<string>(type: "text", nullable: true),
                    address_complement = table.Column<string>(type: "text", nullable: true),
                    email_address = table.Column<string>(type: "text", nullable: false),
                    email_domain = table.Column<string>(type: "text", nullable: false),
                    phone_country_code = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    phone_number = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: true),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    account_status = table.Column<int>(type: "integer", nullable: false),
                    access_level = table.Column<int>(type: "integer", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    suspended_by = table.Column<Guid>(type: "uuid", nullable: true),
                    suspended_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    actived_by = table.Column<Guid>(type: "uuid", nullable: true),
                    actived_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    xmin = table.Column<uint>(type: "xid", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_session",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_agent = table.Column<string>(type: "text", nullable: false),
                    browser = table.Column<string>(type: "text", nullable: true),
                    browser_major = table.Column<string>(type: "text", nullable: true),
                    system = table.Column<string>(type: "text", nullable: true),
                    system_major = table.Column<string>(type: "text", nullable: true),
                    device = table.Column<string>(type: "text", nullable: true),
                    device_brand = table.Column<string>(type: "text", nullable: true),
                    refresh_token = table.Column<string>(type: "text", nullable: false),
                    refresh_created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    refresh_expires_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Is_revoked = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_session", x => x.id);
                    table.ForeignKey(
                        name: "fk_session_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_session_refresh_token",
                table: "user_session",
                column: "refresh_token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_session_user_id",
                table: "user_session",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_cpf",
                table: "users",
                column: "cpf",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_session");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
