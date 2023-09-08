using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Url.Shortener.Data.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "url_metadata",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    short_url = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    full_url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_url_metadata", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_url_metadata_short_url",
                table: "url_metadata",
                column: "short_url",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "url_metadata");
        }
    }
}
