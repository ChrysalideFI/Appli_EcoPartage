using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Appli_EcoPartage.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAnnoncesTagsAndGeoSector : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeographicalSectors_Annonces_IdAnnonce",
                table: "GeographicalSectors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GeographicalSectors",
                table: "GeographicalSectors");

            migrationBuilder.DropIndex(
                name: "IX_GeographicalSectors_IdAnnonce",
                table: "GeographicalSectors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GeographicalSectors",
                table: "GeographicalSectors",
                columns: new[] { "IdAnnonce", "IdGeographicalSector" });

            migrationBuilder.AddForeignKey(
                name: "FK_GeographicalSectors_Annonces_IdAnnonce",
                table: "GeographicalSectors",
                column: "IdAnnonce",
                principalTable: "Annonces",
                principalColumn: "IdAnnonce",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeographicalSectors_Annonces_IdAnnonce",
                table: "GeographicalSectors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GeographicalSectors",
                table: "GeographicalSectors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GeographicalSectors",
                table: "GeographicalSectors",
                column: "IdGeographicalSector");

            migrationBuilder.CreateIndex(
                name: "IX_GeographicalSectors_IdAnnonce",
                table: "GeographicalSectors",
                column: "IdAnnonce");

            migrationBuilder.AddForeignKey(
                name: "FK_GeographicalSectors_Annonces_IdAnnonce",
                table: "GeographicalSectors",
                column: "IdAnnonce",
                principalTable: "Annonces",
                principalColumn: "IdAnnonce",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
