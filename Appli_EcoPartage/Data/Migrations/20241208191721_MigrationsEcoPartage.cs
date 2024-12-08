using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Appli_EcoPartage.Data.Migrations
{
    /// <inheritdoc />
    public partial class MigrationsEcoPartage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    IdTag = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.IdTag);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Annonces",
                columns: table => new
                {
                    IdAnnonce = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    IdUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Annonces", x => x.IdAnnonce);
                    table.ForeignKey(
                        name: "FK_Annonces_Users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    IdComment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdUserGiven = table.Column<int>(type: "int", nullable: false),
                    IdUserRecipient = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.IdComment);
                    table.ForeignKey(
                        name: "FK_Comments_Users_IdUserGiven",
                        column: x => x.IdUserGiven,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Users_IdUserRecipient",
                        column: x => x.IdUserRecipient,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnnoncesTags",
                columns: table => new
                {
                    IdTag = table.Column<int>(type: "int", nullable: false),
                    IdAnnonce = table.Column<int>(type: "int", nullable: false),
                    IdAnnonceTag = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnoncesTags", x => new { x.IdAnnonce, x.IdTag });
                    table.ForeignKey(
                        name: "FK_AnnoncesTags_Annonces_IdAnnonce",
                        column: x => x.IdAnnonce,
                        principalTable: "Annonces",
                        principalColumn: "IdAnnonce",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnnoncesTags_Tags_IdTag",
                        column: x => x.IdTag,
                        principalTable: "Tags",
                        principalColumn: "IdTag",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeographicalSectors",
                columns: table => new
                {
                    IdGeographicalSector = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAnnonce = table.Column<int>(type: "int", nullable: false),
                    FirstPlace = table.Column<int>(type: "int", nullable: false),
                    SecondPlace = table.Column<int>(type: "int", nullable: false),
                    ThirdPlace = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeographicalSectors", x => x.IdGeographicalSector);
                    table.ForeignKey(
                        name: "FK_GeographicalSectors_Annonces_IdAnnonce",
                        column: x => x.IdAnnonce,
                        principalTable: "Annonces",
                        principalColumn: "IdAnnonce",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    IdTransaction = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserIdGiver = table.Column<int>(type: "int", nullable: false),
                    UserIdRecipient = table.Column<int>(type: "int", nullable: false),
                    IdAnnonce = table.Column<int>(type: "int", nullable: false),
                    AnnoncePoint = table.Column<int>(type: "int", nullable: false),
                    DateTransaction = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.IdTransaction);
                    table.ForeignKey(
                        name: "FK_Transactions_Annonces_AnnoncePoint",
                        column: x => x.AnnoncePoint,
                        principalTable: "Annonces",
                        principalColumn: "IdAnnonce",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Annonces_IdAnnonce",
                        column: x => x.IdAnnonce,
                        principalTable: "Annonces",
                        principalColumn: "IdAnnonce",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Users_UserIdGiver",
                        column: x => x.UserIdGiver,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Users_UserIdRecipient",
                        column: x => x.UserIdRecipient,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Annonces_IdUser",
                table: "Annonces",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_AnnoncesTags_IdTag",
                table: "AnnoncesTags",
                column: "IdTag");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_IdUserGiven",
                table: "Comments",
                column: "IdUserGiven");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_IdUserRecipient",
                table: "Comments",
                column: "IdUserRecipient");

            migrationBuilder.CreateIndex(
                name: "IX_GeographicalSectors_IdAnnonce",
                table: "GeographicalSectors",
                column: "IdAnnonce");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AnnoncePoint",
                table: "Transactions",
                column: "AnnoncePoint");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_IdAnnonce",
                table: "Transactions",
                column: "IdAnnonce");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserIdGiver",
                table: "Transactions",
                column: "UserIdGiver");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserIdRecipient",
                table: "Transactions",
                column: "UserIdRecipient");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnoncesTags");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "GeographicalSectors");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Annonces");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
