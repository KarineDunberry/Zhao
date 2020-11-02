using Microsoft.EntityFrameworkCore.Migrations;

namespace TP1_KarineDunberry.Migrations
{
    public partial class AjoutStatutReservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatutReservation",
                table: "Reservation",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatutReservation",
                table: "Reservation");
        }
    }
}
