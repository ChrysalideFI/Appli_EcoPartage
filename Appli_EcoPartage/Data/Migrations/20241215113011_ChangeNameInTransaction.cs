using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Appli_EcoPartage.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNameInTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AspNetUsers_UserIdGiver",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AspNetUsers_UserIdRecipient",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "UserIdRecipient",
                table: "Transactions",
                newName: "UserIdSeller");

            migrationBuilder.RenameColumn(
                name: "UserIdGiver",
                table: "Transactions",
                newName: "UserIdBuyer");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_UserIdRecipient",
                table: "Transactions",
                newName: "IX_Transactions_UserIdSeller");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_UserIdGiver",
                table: "Transactions",
                newName: "IX_Transactions_UserIdBuyer");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AspNetUsers_UserIdBuyer",
                table: "Transactions",
                column: "UserIdBuyer",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AspNetUsers_UserIdSeller",
                table: "Transactions",
                column: "UserIdSeller",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AspNetUsers_UserIdBuyer",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AspNetUsers_UserIdSeller",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "UserIdSeller",
                table: "Transactions",
                newName: "UserIdRecipient");

            migrationBuilder.RenameColumn(
                name: "UserIdBuyer",
                table: "Transactions",
                newName: "UserIdGiver");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_UserIdSeller",
                table: "Transactions",
                newName: "IX_Transactions_UserIdRecipient");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_UserIdBuyer",
                table: "Transactions",
                newName: "IX_Transactions_UserIdGiver");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AspNetUsers_UserIdGiver",
                table: "Transactions",
                column: "UserIdGiver",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AspNetUsers_UserIdRecipient",
                table: "Transactions",
                column: "UserIdRecipient",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
