using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SM.Application.Migrations
{
    /// <inheritdoc />
    public partial class PostAndCommentAbstraction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Message_ParentPostId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Users_UserId1",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "Message",
                newName: "ParentCommentId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_UserId1",
                table: "Message",
                newName: "IX_Message_ParentCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Message_ParentCommentId",
                table: "Message",
                column: "ParentCommentId",
                principalTable: "Message",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Message_ParentPostId",
                table: "Message",
                column: "ParentPostId",
                principalTable: "Message",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Message_ParentCommentId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Message_ParentPostId",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "ParentCommentId",
                table: "Message",
                newName: "UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_Message_ParentCommentId",
                table: "Message",
                newName: "IX_Message_UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Message_ParentPostId",
                table: "Message",
                column: "ParentPostId",
                principalTable: "Message",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Users_UserId1",
                table: "Message",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
