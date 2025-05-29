using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auction.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateCreate",
                table: "Product",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "DateStart",
                table: "Auction",
                newName: "Start");

            migrationBuilder.RenameColumn(
                name: "DateEnd",
                table: "Auction",
                newName: "End");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Product",
                newName: "DateCreate");

            migrationBuilder.RenameColumn(
                name: "Start",
                table: "Auction",
                newName: "DateStart");

            migrationBuilder.RenameColumn(
                name: "End",
                table: "Auction",
                newName: "DateEnd");
        }
    }
}
