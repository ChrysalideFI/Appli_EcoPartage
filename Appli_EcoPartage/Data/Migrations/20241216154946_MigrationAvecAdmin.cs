using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Appli_EcoPartage.Data.Migrations
{
    /// <inheritdoc />
    public partial class MigrationAvecAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsValidated",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsValidated",
                table: "Annonces",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsValidated",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsValidated",
                table: "Annonces");
        }
    }
}
