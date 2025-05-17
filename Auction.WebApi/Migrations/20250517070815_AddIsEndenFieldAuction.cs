using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auction.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddIsEndenFieldAuction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnded",
                table: "Auction",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnded",
                table: "Auction");
        }
    }
}
