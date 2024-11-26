using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektInzynierski.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReservationSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Equipments_EquipmentID",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Equipments",
                table: "Equipments");

            migrationBuilder.RenameTable(
                name: "Equipments",
                newName: "Equipment");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Equipment",
                table: "Equipment",
                column: "EquipmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Equipment_EquipmentID",
                table: "Reservations",
                column: "EquipmentID",
                principalTable: "Equipment",
                principalColumn: "EquipmentID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Equipment_EquipmentID",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Equipment",
                table: "Equipment");

            migrationBuilder.RenameTable(
                name: "Equipment",
                newName: "Equipments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Equipments",
                table: "Equipments",
                column: "EquipmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Equipments_EquipmentID",
                table: "Reservations",
                column: "EquipmentID",
                principalTable: "Equipments",
                principalColumn: "EquipmentID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
