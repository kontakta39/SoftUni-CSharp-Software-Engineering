using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MusicWebStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditedManyToManyRelationshipTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("4598fddb-d060-497a-be37-9bc24c52827c"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("6d20be91-912a-418c-a0b1-f5f3d7a07b3c"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("b74388a4-9d0f-41ca-b3e6-5f2838ce740c"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("eb18ee21-3f7d-4240-b502-fc38d24487e3"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("f6f49c27-f0ee-46ba-9612-2c7e07a957ed"));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { new Guid("4598fddb-d060-497a-be37-9bc24c52827c"), false, "Heavy Metal" },
                    { new Guid("6d20be91-912a-418c-a0b1-f5f3d7a07b3c"), false, "Rock" },
                    { new Guid("b74388a4-9d0f-41ca-b3e6-5f2838ce740c"), false, "Pop" },
                    { new Guid("eb18ee21-3f7d-4240-b502-fc38d24487e3"), false, "Blues" },
                    { new Guid("f6f49c27-f0ee-46ba-9612-2c7e07a957ed"), false, "Jazz" }
                });
        }
    }
}
