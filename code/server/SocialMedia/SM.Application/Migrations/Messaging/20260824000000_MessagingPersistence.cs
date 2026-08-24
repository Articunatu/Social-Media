using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SM.Application.Migrations.Messaging;

[Migration("20260824000000_MessagingPersistence")]
public partial class MessagingPersistence : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Conversations",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Conversations", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ConversationParticipants",
            columns: table => new
            {
                ConversationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ConversationParticipants", x => new { x.ConversationId, x.UserId });
                table.ForeignKey(
                    name: "FK_ConversationParticipants_Conversations_ConversationId",
                    column: x => x.ConversationId,
                    principalTable: "Conversations",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "DirectMessages",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                TimeStamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ConversationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                TimeOfDelete = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DirectMessages", x => x.Id);
                table.ForeignKey(
                    name: "FK_DirectMessages_Conversations_ConversationId",
                    column: x => x.ConversationId,
                    principalTable: "Conversations",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ConversationParticipants_UserId",
            table: "ConversationParticipants",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_DirectMessages_ConversationId_TimeStamp",
            table: "DirectMessages",
            columns: ["ConversationId", "TimeStamp"]);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "DirectMessages");
        migrationBuilder.DropTable(name: "ConversationParticipants");
        migrationBuilder.DropTable(name: "Conversations");
    }
}