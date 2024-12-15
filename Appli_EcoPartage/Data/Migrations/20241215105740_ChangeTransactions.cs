using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Appli_EcoPartage.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Annonces_AnnoncePoint",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_AnnoncePoint",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "AnnoncePoint",
                table: "Transactions");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Transactions");

            migrationBuilder.AddColumn<int>(
                name: "AnnoncePoint",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AnnoncePoint",
                table: "Transactions",
                column: "AnnoncePoint");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Annonces_AnnoncePoint",
                table: "Transactions",
                column: "AnnoncePoint",
                principalTable: "Annonces",
                principalColumn: "IdAnnonce",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
