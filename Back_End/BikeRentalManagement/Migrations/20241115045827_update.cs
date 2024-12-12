using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BikeRentalManagement.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BikeUnits_Bikes_BikeID",
                table: "BikeUnits");

            migrationBuilder.RenameColumn(
                name: "BikeID",
                table: "BikeUnits",
                newName: "BikeId");

            migrationBuilder.RenameIndex(
                name: "IX_BikeUnits_BikeID",
                table: "BikeUnits",
                newName: "IX_BikeUnits_BikeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BikeUnits_Bikes_BikeId",
                table: "BikeUnits",
                column: "BikeId",
                principalTable: "Bikes",
                principalColumn: "BikeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BikeUnits_Bikes_BikeId",
                table: "BikeUnits");

            migrationBuilder.RenameColumn(
                name: "BikeId",
                table: "BikeUnits",
                newName: "BikeID");

            migrationBuilder.RenameIndex(
                name: "IX_BikeUnits_BikeId",
                table: "BikeUnits",
                newName: "IX_BikeUnits_BikeID");

            migrationBuilder.AddForeignKey(
                name: "FK_BikeUnits_Bikes_BikeID",
                table: "BikeUnits",
                column: "BikeID",
                principalTable: "Bikes",
                principalColumn: "BikeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
