using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateConsignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FishStatusId",
                table: "FishConsignments");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "FishConsignments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "FishConsignments");

            migrationBuilder.AddColumn<int>(
                name: "FishStatusId",
                table: "FishConsignments",
                type: "int",
                nullable: true);
        }
    }
}
