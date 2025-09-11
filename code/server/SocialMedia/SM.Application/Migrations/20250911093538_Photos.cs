using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SM.Application.Migrations
{
    /// <inheritdoc />
    public partial class Photos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photo_Users_UserId",
                table: "Photo");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Photo_ProfilePhotoId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Photo",
                table: "Photo");

            migrationBuilder.RenameTable(
                name: "Photo",
                newName: "Photos");

            migrationBuilder.RenameColumn(
                name: "PhotoUrl",
                table: "Photos",
                newName: "FileName");

            migrationBuilder.RenameIndex(
                name: "IX_Photo_UserId",
                table: "Photos",
                newName: "IX_Photos_UserId");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordSalt",
                table: "Users",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "Users",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Photos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                table: "Photos",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photos",
                table: "Photos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Users_UserId",
                table: "Photos",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Photos_ProfilePhotoId",
                table: "Users",
                column: "ProfilePhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Users_UserId",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Photos_ProfilePhotoId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Photos",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "Data",
                table: "Photos");

            migrationBuilder.RenameTable(
                name: "Photos",
                newName: "Photo");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Photo",
                newName: "PhotoUrl");

            migrationBuilder.RenameIndex(
                name: "IX_Photos_UserId",
                table: "Photo",
                newName: "IX_Photo_UserId");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordSalt",
                table: "Users",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "Users",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photo",
                table: "Photo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Photo_Users_UserId",
                table: "Photo",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Photo_ProfilePhotoId",
                table: "Users",
                column: "ProfilePhotoId",
                principalTable: "Photo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
