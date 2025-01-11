using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektInzynierski.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestinUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequestingUserId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestingUserId",
                table: "Reservations");
        }
    }
}
