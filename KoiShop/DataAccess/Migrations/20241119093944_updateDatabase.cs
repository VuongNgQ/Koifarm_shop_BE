using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFromShop",
                table: "FishConsignments");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "FishConsignments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "userFishOwnerships",
                columns: table => new
                {
                    OwnershipId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FishId = table.Column<int>(type: "int", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userFishOwnerships", x => x.OwnershipId);
                    table.ForeignKey(
                        name: "FK_userFishOwnerships_Fish_FishId",
                        column: x => x.FishId,
                        principalTable: "Fish",
                        principalColumn: "FishId");
                    table.ForeignKey(
                        name: "FK_userFishOwnerships_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_userFishOwnerships_FishId",
                table: "userFishOwnerships",
                column: "FishId");

            migrationBuilder.CreateIndex(
                name: "IX_userFishOwnerships_UserId",
                table: "userFishOwnerships",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "userFishOwnerships");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "FishConsignments");

            migrationBuilder.AddColumn<bool>(
                name: "IsFromShop",
                table: "FishConsignments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
