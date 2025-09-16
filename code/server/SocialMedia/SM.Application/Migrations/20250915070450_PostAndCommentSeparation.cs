using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SM.Application.Migrations
{
    /// <inheritdoc />
    public partial class PostAndCommentSeparation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reactions_MessageId",
                table: "Reactions");


            migrationBuilder.CreateIndex(
                name: "IX_Reactions_MessageId_UserId",
                table: "Reactions",
                columns: new[] { "MessageId", "UserId" },
                unique: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reactions_MessageId_UserId",
                table: "Reactions");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_MessageId",
                table: "Reactions",
                column: "MessageId");
        }
    }
}
