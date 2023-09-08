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
                name: "UrlMetadata",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ShortUrl = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FullUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlMetadata", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UrlMetadata_ShortUrl",
                table: "UrlMetadata",
                column: "ShortUrl",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UrlMetadata");
        }
    }
}
