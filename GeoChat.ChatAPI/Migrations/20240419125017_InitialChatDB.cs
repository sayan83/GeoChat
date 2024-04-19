using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GeoChat.ChatAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialChatDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    ChatId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoomId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    SendTime = table.Column<long>(type: "INTEGER", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.ChatId);
                });

            migrationBuilder.InsertData(
                table: "Chats",
                columns: new[] { "ChatId", "Message", "RoomId", "SendTime", "UserId" },
                values: new object[,]
                {
                    { new Guid("0b62f0c5-f566-4700-b122-9f852eedf07e"), "Toki for u boss ...", new Guid("65958d08-4e33-40da-a0ac-54442fba0abe"), 1706896238L, "slave1" },
                    { new Guid("3362357a-2811-4100-9207-bf77e5b991ed"), "Good! Get me some whiskey", new Guid("65958d08-4e33-40da-a0ac-54442fba0abe"), 1706896187L, "sayan83" },
                    { new Guid("c0bc0ef8-5708-492f-aa49-da2d150e2835"), "Hi, I am your new agent.", new Guid("0d0b9a01-7246-4fb1-ac70-2f2c5d5f084b"), 1706942491L, "agent1" },
                    { new Guid("c68605be-7e12-4710-ae48-bf5d70c89868"), "Hi Okk ...", new Guid("0d0b9a01-7246-4fb1-ac70-2f2c5d5f084b"), 1706942611L, "slave1" },
                    { new Guid("d44facf9-d3f3-4f2b-baa0-15d62ed3d656"), "Ok Boss", new Guid("65958d08-4e33-40da-a0ac-54442fba0abe"), 1706882333L, "slave1" },
                    { new Guid("e0bc032f-5728-484f-ad49-da2d150e5415"), "U have been declared a slave henceforth", new Guid("65958d08-4e33-40da-a0ac-54442fba0abe"), 1706882313L, "sayan83" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chats");
        }
    }
}
