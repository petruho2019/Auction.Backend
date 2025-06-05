using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auction.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDateInAuctionParticipation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Auction");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "User",
                newSchema: "Auction");

            migrationBuilder.RenameTable(
                name: "ProductImage",
                newName: "ProductImage",
                newSchema: "Auction");

            migrationBuilder.RenameTable(
                name: "Product",
                newName: "Product",
                newSchema: "Auction");

            migrationBuilder.RenameTable(
                name: "AuctionParticipation",
                newName: "AuctionParticipation",
                newSchema: "Auction");

            migrationBuilder.RenameTable(
                name: "Auction",
                newName: "Auction",
                newSchema: "Auction");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                schema: "Auction",
                table: "Product",
                type: "TIMESTAMP",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BidTime",
                schema: "Auction",
                table: "AuctionParticipation",
                type: "TIMESTAMP",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Start",
                schema: "Auction",
                table: "Auction",
                type: "TIMESTAMP",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "End",
                schema: "Auction",
                table: "Auction",
                type: "TIMESTAMP",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                schema: "Auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    CreatedByIp = table.Column<string>(type: "text", nullable: false),
                    Expires = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    Created = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    Revoked = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Refreshtoken_pkey", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_User_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "Auction",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_OwnerId",
                schema: "Auction",
                table: "RefreshToken",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshToken",
                schema: "Auction");

            migrationBuilder.RenameTable(
                name: "User",
                schema: "Auction",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "ProductImage",
                schema: "Auction",
                newName: "ProductImage");

            migrationBuilder.RenameTable(
                name: "Product",
                schema: "Auction",
                newName: "Product");

            migrationBuilder.RenameTable(
                name: "AuctionParticipation",
                schema: "Auction",
                newName: "AuctionParticipation");

            migrationBuilder.RenameTable(
                name: "Auction",
                schema: "Auction",
                newName: "Auction");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Product",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BidTime",
                table: "AuctionParticipation",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Start",
                table: "Auction",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "End",
                table: "Auction",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP");
        }
    }
}
