using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeoChat.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class ModifyRoomDbToIncludeLocAndRange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Rooms",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Rooms",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Range",
                table: "Rooms",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: new Guid("0d0b9a01-7246-4fb1-ac70-2f2c5d5f084b"),
                columns: new[] { "Latitude", "Longitude", "Range" },
                values: new object[] { 22.5869952, 88.439311099999998, 5000 });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: new Guid("65958d08-4e33-40da-a0ac-54442fba0abe"),
                columns: new[] { "Latitude", "Longitude", "Range" },
                values: new object[] { 22.5869952, 88.439311099999998, 1000 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Range",
                table: "Rooms");
        }
    }
}
