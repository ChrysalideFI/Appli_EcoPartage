using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Appli_EcoPartage.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPKAnnoncesTagsAndAnnonceGeoSector : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AnnoncesTags",
                table: "AnnoncesTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnnoncesGeoSectors",
                table: "AnnoncesGeoSectors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnnoncesTags",
                table: "AnnoncesTags",
                columns: new[] { "IdAnnonce", "IdTag", "IdAnnonceTag" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnnoncesGeoSectors",
                table: "AnnoncesGeoSectors",
                columns: new[] { "IdAnnonce", "IdGeographicalSector", "IdAnnoncesGeoSector" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AnnoncesTags",
                table: "AnnoncesTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnnoncesGeoSectors",
                table: "AnnoncesGeoSectors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnnoncesTags",
                table: "AnnoncesTags",
                columns: new[] { "IdAnnonce", "IdTag" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnnoncesGeoSectors",
                table: "AnnoncesGeoSectors",
                columns: new[] { "IdAnnonce", "IdGeographicalSector" });
        }
    }
}
