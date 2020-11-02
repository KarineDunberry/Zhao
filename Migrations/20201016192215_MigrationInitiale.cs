using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TP1_KarineDunberry.Migrations
{
    public partial class MigrationInitiale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Evaluation",
                columns: table => new
                {
                    EvaluationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Prénom = table.Column<string>(maxLength: 100, nullable: false),
                    Nom = table.Column<string>(maxLength: 100, nullable: false),
                    TypeReservation = table.Column<int>(nullable: false),
                    Courriel = table.Column<string>(nullable: true),
                    DateVisite = table.Column<DateTime>(nullable: false),
                    QualitéRepas = table.Column<int>(nullable: false),
                    QualitéService = table.Column<int>(nullable: false),
                    Commentaires = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evaluation", x => x.EvaluationID);
                });

            migrationBuilder.CreateTable(
                name: "Promotion",
                columns: table => new
                {
                    PromotionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypePromotion = table.Column<int>(nullable: false),
                    TauxApplicable = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    DateDébut = table.Column<DateTime>(nullable: false),
                    DateFin = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotion", x => x.PromotionID);
                });

            migrationBuilder.CreateTable(
                name: "Reservation",
                columns: table => new
                {
                    ReservationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Prénom = table.Column<string>(maxLength: 100, nullable: false),
                    Nom = table.Column<string>(maxLength: 100, nullable: false),
                    TypeReservation = table.Column<int>(nullable: false),
                    Courriel = table.Column<string>(nullable: true),
                    DateHeure = table.Column<DateTime>(nullable: false),
                    Téléphone = table.Column<string>(nullable: false),
                    nbPersonnes = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => x.ReservationID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Evaluation");

            migrationBuilder.DropTable(
                name: "Promotion");

            migrationBuilder.DropTable(
                name: "Reservation");
        }
    }
}
