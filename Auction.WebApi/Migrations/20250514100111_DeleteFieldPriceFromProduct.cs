using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auction.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class DeleteFieldPriceFromProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Product");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Price",
                table: "Product",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
