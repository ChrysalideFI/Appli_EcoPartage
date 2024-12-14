using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Appli_EcoPartage.Data.Migrations
{
    /// <inheritdoc />
    public partial class changeEcoPartage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Annonces_AspNetUsers_IdUser",
                table: "Annonces");

            migrationBuilder.DropForeignKey(
                name: "FK_AnnoncesTags_Annonces_IdAnnonce",
                table: "AnnoncesTags");

            migrationBuilder.DropForeignKey(
                name: "FK_GeographicalSectors_Annonces_IdAnnonce",
                table: "GeographicalSectors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GeographicalSectors",
                table: "GeographicalSectors");

            migrationBuilder.DropColumn(
                name: "IdAnnonce",
                table: "GeographicalSectors");

            migrationBuilder.DropColumn(
                name: "FirstPlace",
                table: "GeographicalSectors");

            migrationBuilder.DropColumn(
                name: "SecondPlace",
                table: "GeographicalSectors");

            migrationBuilder.RenameColumn(
                name: "ThirdPlace",
                table: "GeographicalSectors",
                newName: "Place");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GeographicalSectors",
                table: "GeographicalSectors",
                column: "IdGeographicalSector");

            migrationBuilder.CreateTable(
                name: "AnnoncesGeoSectors",
                columns: table => new
                {
                    IdGeographicalSector = table.Column<int>(type: "int", nullable: false),
                    IdAnnonce = table.Column<int>(type: "int", nullable: false),
                    IdAnnoncesGeoSector = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnoncesGeoSectors", x => new { x.IdAnnonce, x.IdGeographicalSector });
                    table.ForeignKey(
                        name: "FK_AnnoncesGeoSectors_Annonces_IdAnnonce",
                        column: x => x.IdAnnonce,
                        principalTable: "Annonces",
                        principalColumn: "IdAnnonce",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnnoncesGeoSectors_GeographicalSectors_IdGeographicalSector",
                        column: x => x.IdGeographicalSector,
                        principalTable: "GeographicalSectors",
                        principalColumn: "IdGeographicalSector",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnnoncesGeoSectors_IdGeographicalSector",
                table: "AnnoncesGeoSectors",
                column: "IdGeographicalSector");

            migrationBuilder.AddForeignKey(
                name: "FK_Annonces_AspNetUsers_IdUser",
                table: "Annonces",
                column: "IdUser",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AnnoncesTags_Annonces_IdAnnonce",
                table: "AnnoncesTags",
                column: "IdAnnonce",
                principalTable: "Annonces",
                principalColumn: "IdAnnonce",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Annonces_AspNetUsers_IdUser",
                table: "Annonces");

            migrationBuilder.DropForeignKey(
                name: "FK_AnnoncesTags_Annonces_IdAnnonce",
                table: "AnnoncesTags");

            migrationBuilder.DropTable(
                name: "AnnoncesGeoSectors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GeographicalSectors",
                table: "GeographicalSectors");

            migrationBuilder.RenameColumn(
                name: "Place",
                table: "GeographicalSectors",
                newName: "ThirdPlace");

            migrationBuilder.AddColumn<int>(
                name: "IdAnnonce",
                table: "GeographicalSectors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FirstPlace",
                table: "GeographicalSectors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SecondPlace",
                table: "GeographicalSectors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GeographicalSectors",
                table: "GeographicalSectors",
                columns: new[] { "IdAnnonce", "IdGeographicalSector" });

            migrationBuilder.AddForeignKey(
                name: "FK_Annonces_AspNetUsers_IdUser",
                table: "Annonces",
                column: "IdUser",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnnoncesTags_Annonces_IdAnnonce",
                table: "AnnoncesTags",
                column: "IdAnnonce",
                principalTable: "Annonces",
                principalColumn: "IdAnnonce",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GeographicalSectors_Annonces_IdAnnonce",
                table: "GeographicalSectors",
                column: "IdAnnonce",
                principalTable: "Annonces",
                principalColumn: "IdAnnonce",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
