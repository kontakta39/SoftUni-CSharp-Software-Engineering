using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookWebStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsReturnedPropertyToOrderBookClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                 name: "IsReturned",
                 table: "OrdersBooks",
                 type: "bit",
                 nullable: false,
                 defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                 name: "IsReturned",
                 table: "OrdersBooks");
        }
    }
}
