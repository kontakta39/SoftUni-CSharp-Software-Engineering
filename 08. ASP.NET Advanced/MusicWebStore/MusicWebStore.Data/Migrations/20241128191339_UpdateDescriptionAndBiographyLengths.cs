using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MusicWebStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDescriptionAndBiographyLengths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("1b3b133e-e588-445c-9987-040901e71a7b"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("2b7b51e4-bf90-48cc-a44a-c3badad23927"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("a7307d5a-e358-4be0-b0bc-597abb4ef224"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("a9707227-27c6-4347-9e6e-f8b4ce4cef36"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("f2fa2c9c-f40a-4cd9-bc62-a910d03ecb62"));

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { new Guid("0cc8f320-921a-4686-aa49-3b7d6cd13ee0"), false, "Jazz" },
                    { new Guid("6d2c74a1-aefb-473a-ba56-96b81bb25ae1"), false, "Blues" },
                    { new Guid("c4099fb5-153a-4422-ad93-9354885fa733"), false, "Rock" },
                    { new Guid("c6f1caf5-1b0d-42e5-ab9e-9c993cfef82b"), false, "Pop" },
                    { new Guid("f0fdb01e-fe18-4e79-bf1c-300e3b06f0c6"), false, "Heavy Metal" }
                });

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Albums",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 500); 

            migrationBuilder.AlterColumn<string>(
                name: "Biography",
                table: "Artists",
                maxLength: 1000, 
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 500);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("0cc8f320-921a-4686-aa49-3b7d6cd13ee0"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("6d2c74a1-aefb-473a-ba56-96b81bb25ae1"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("c4099fb5-153a-4422-ad93-9354885fa733"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("c6f1caf5-1b0d-42e5-ab9e-9c993cfef82b"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("f0fdb01e-fe18-4e79-bf1c-300e3b06f0c6"));

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { new Guid("1b3b133e-e588-445c-9987-040901e71a7b"), false, "Heavy Metal" },
                    { new Guid("2b7b51e4-bf90-48cc-a44a-c3badad23927"), false, "Blues" },
                    { new Guid("a7307d5a-e358-4be0-b0bc-597abb4ef224"), false, "Pop" },
                    { new Guid("a9707227-27c6-4347-9e6e-f8b4ce4cef36"), false, "Jazz" },
                    { new Guid("f2fa2c9c-f40a-4cd9-bc62-a910d03ecb62"), false, "Rock" }
                });

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Albums",
                maxLength: 500, 
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "Biography",
                table: "Artists",
                maxLength: 500, 
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 1000); 
        }
    }
}
