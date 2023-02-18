using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaramaMVC.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnaBilimDals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnaBilimDals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Personels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SurName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AnaBilimDallariId = table.Column<int>(type: "int", nullable: false),
                    User = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Alintilanma = table.Column<int>(type: "int", nullable: false),
                    ScholarName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Personels_AnaBilimDals_AnaBilimDallariId",
                        column: x => x.AnaBilimDallariId,
                        principalTable: "AnaBilimDals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonelYayinBilgileris",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonelId = table.Column<int>(type: "int", nullable: false),
                    Baslik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BaslikCites = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alinti = table.Column<int>(type: "int", nullable: false),
                    Yil = table.Column<int>(type: "int", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonelYayinBilgileris", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonelYayinBilgileris_Personels_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YayinAlintiBilgisis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YayinId = table.Column<int>(type: "int", nullable: false),
                    PersonelYayinBilgileriId = table.Column<int>(type: "int", nullable: true),
                    Tip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YayinAlintiBilgisis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YayinAlintiBilgisis_PersonelYayinBilgileris_PersonelYayinBilgileriId",
                        column: x => x.PersonelYayinBilgileriId,
                        principalTable: "PersonelYayinBilgileris",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Personels_AnaBilimDallariId",
                table: "Personels",
                column: "AnaBilimDallariId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelYayinBilgileris_PersonelId",
                table: "PersonelYayinBilgileris",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_YayinAlintiBilgisis_PersonelYayinBilgileriId",
                table: "YayinAlintiBilgisis",
                column: "PersonelYayinBilgileriId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "YayinAlintiBilgisis");

            migrationBuilder.DropTable(
                name: "PersonelYayinBilgileris");

            migrationBuilder.DropTable(
                name: "Personels");

            migrationBuilder.DropTable(
                name: "AnaBilimDals");
        }
    }
}
