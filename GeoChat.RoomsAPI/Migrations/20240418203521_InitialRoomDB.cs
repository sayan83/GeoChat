using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GeoChat.RoomsAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialRoomDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoomName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    Range = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomId);
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    RoomId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => new { x.RoomId, x.UserId });
                    table.ForeignKey(
                        name: "FK_Participants_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "Description", "Latitude", "Longitude", "Range", "RoomName", "UserId" },
                values: new object[,]
                {
                    { new Guid("0d0b9a01-7246-4fb1-ac70-2f2c5d5f084b"), "My first room", 22.5869952, 88.439311099999998, 5000, "Room1", "agent1" },
                    { new Guid("65958d08-4e33-40da-a0ac-54442fba0abe"), "Private King room", 22.5869952, 88.439311099999998, 1000, "Kings Room", "sayan83" }
                });

            migrationBuilder.InsertData(
                table: "Participants",
                columns: new[] { "RoomId", "UserId" },
                values: new object[,]
                {
                    { new Guid("0d0b9a01-7246-4fb1-ac70-2f2c5d5f084b"), "agent1" },
                    { new Guid("0d0b9a01-7246-4fb1-ac70-2f2c5d5f084b"), "sayan83" },
                    { new Guid("0d0b9a01-7246-4fb1-ac70-2f2c5d5f084b"), "slave1" },
                    { new Guid("65958d08-4e33-40da-a0ac-54442fba0abe"), "sayan83" },
                    { new Guid("65958d08-4e33-40da-a0ac-54442fba0abe"), "slave1" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
