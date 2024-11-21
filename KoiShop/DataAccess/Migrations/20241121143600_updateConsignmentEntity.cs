using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateConsignmentEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransferDate",
                table: "FishConsignments",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "ReceiveDate",
                table: "FishConsignments",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "FishConsignments",
                newName: "ServiceFee");

            migrationBuilder.AlterColumn<int>(
                name: "FishId",
                table: "FishConsignments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CommissionFee",
                table: "FishConsignments",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FinalPrice",
                table: "FishConsignments",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "InitialPrice",
                table: "FishConsignments",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Video",
                table: "FishConsignments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommissionFee",
                table: "FishConsignments");

            migrationBuilder.DropColumn(
                name: "FinalPrice",
                table: "FishConsignments");

            migrationBuilder.DropColumn(
                name: "InitialPrice",
                table: "FishConsignments");

            migrationBuilder.DropColumn(
                name: "Video",
                table: "FishConsignments");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "FishConsignments",
                newName: "TransferDate");

            migrationBuilder.RenameColumn(
                name: "ServiceFee",
                table: "FishConsignments",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "FishConsignments",
                newName: "ReceiveDate");

            migrationBuilder.AlterColumn<int>(
                name: "FishId",
                table: "FishConsignments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
