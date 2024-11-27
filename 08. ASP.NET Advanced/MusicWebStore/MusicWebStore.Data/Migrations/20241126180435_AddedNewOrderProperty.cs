using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MusicWebStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewOrderProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("2a83dccd-f530-440b-894d-a7cf1c1ea01d"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("8a45c04e-56d9-442a-87d2-e87226a0c63c"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("a55d1b0a-c671-411c-a48f-b1a24e2ed51e"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("a72f3411-e21a-4e6d-8a22-1ac09fcfa859"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("a75bd6e1-7569-410c-9f14-cbc9681b6fd0"));

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { new Guid("2a0e34f1-b982-4700-b0cb-4f6fa24d5080"), false, "Blues" },
                    { new Guid("5451d285-6c0a-4f3e-a77a-8d1648811416"), false, "Jazz" },
                    { new Guid("ad6be224-41e7-4f8e-9157-b84e6528b416"), false, "Rock" },
                    { new Guid("ba87ceb3-3479-41ef-872f-ea42f353e4c7"), false, "Pop" },
                    { new Guid("dc87ef16-3a57-4516-9f2c-cf9fd08a5ca8"), false, "Heavy Metal" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("2a0e34f1-b982-4700-b0cb-4f6fa24d5080"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("5451d285-6c0a-4f3e-a77a-8d1648811416"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("ad6be224-41e7-4f8e-9157-b84e6528b416"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("ba87ceb3-3479-41ef-872f-ea42f353e4c7"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("dc87ef16-3a57-4516-9f2c-cf9fd08a5ca8"));

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Orders");

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { new Guid("2a83dccd-f530-440b-894d-a7cf1c1ea01d"), false, "Blues" },
                    { new Guid("8a45c04e-56d9-442a-87d2-e87226a0c63c"), false, "Rock" },
                    { new Guid("a55d1b0a-c671-411c-a48f-b1a24e2ed51e"), false, "Jazz" },
                    { new Guid("a72f3411-e21a-4e6d-8a22-1ac09fcfa859"), false, "Heavy Metal" },
                    { new Guid("a75bd6e1-7569-410c-9f14-cbc9681b6fd0"), false, "Pop" }
                });
        }
    }
}
