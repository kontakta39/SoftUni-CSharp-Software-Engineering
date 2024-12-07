using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MusicWebStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsEditedAndIsReturnedProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add IsEdited column to Reviews table
            migrationBuilder.AddColumn<bool>(
                name: "IsEdited",
                table: "Reviews",
                type: "bit",
                nullable: false,
                defaultValue: false);

            // Add isReturned column to OrderAlbums table
            migrationBuilder.AddColumn<bool>(
                name: "isReturned",
                table: "OrdersAlbums",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove IsEdited column from Reviews table
            migrationBuilder.DropColumn(
                name: "IsEdited",
                table: "Reviews");

            // Remove isReturned column from OrderAlbums table
            migrationBuilder.DropColumn(
                name: "isReturned",
                table: "OrdersAlbums");
        }
    }
}
