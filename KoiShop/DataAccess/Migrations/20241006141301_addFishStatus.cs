using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addFishStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FishStatuses",
                columns: table => new
                {
                    FishStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FishStatuses", x => x.FishStatusId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PackageConsignments_PackageStatusId",
                table: "PackageConsignments",
                column: "PackageStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_FishConsignments_FishStatusId",
                table: "FishConsignments",
                column: "FishStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_FishConsignments_FishStatuses_FishStatusId",
                table: "FishConsignments",
                column: "FishStatusId",
                principalTable: "FishStatuses",
                principalColumn: "FishStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_PackageConsignments_FishStatuses_PackageStatusId",
                table: "PackageConsignments",
                column: "PackageStatusId",
                principalTable: "FishStatuses",
                principalColumn: "FishStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FishConsignments_FishStatuses_FishStatusId",
                table: "FishConsignments");

            migrationBuilder.DropForeignKey(
                name: "FK_PackageConsignments_FishStatuses_PackageStatusId",
                table: "PackageConsignments");

            migrationBuilder.DropTable(
                name: "FishStatuses");

            migrationBuilder.DropIndex(
                name: "IX_PackageConsignments_PackageStatusId",
                table: "PackageConsignments");

            migrationBuilder.DropIndex(
                name: "IX_FishConsignments_FishStatusId",
                table: "FishConsignments");
        }
    }
}
