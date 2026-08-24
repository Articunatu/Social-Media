using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SM.Application.Migrations.Feed
{
    /// <inheritdoc />
    public partial class FeedProjection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserFeedItems",
                columns: table => new
                {
                    RecipientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFeedItems", x => new { x.RecipientId, x.PostId });
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFeedItems_RecipientId_CreatedAt",
                table: "UserFeedItems",
                columns: new[] { "RecipientId", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFeedItems");
        }
    }
}
