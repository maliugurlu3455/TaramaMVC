using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaramaMVC.Migrations
{
    /// <inheritdoc />
    public partial class Mig1 : Migration
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
                name: "YayinBilgisi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Baslik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alinti = table.Column<int>(type: "int", nullable: false),
                    Yil = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YayinBilgisi", x => x.Id);
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
                    PersonelId = table.Column<int>(type: "int", nullable: true),
                    YayinBilgisiId = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonelYayinBilgileris", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonelYayinBilgileris_Personels_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PersonelYayinBilgileris_YayinBilgisi_YayinBilgisiId",
                        column: x => x.YayinBilgisiId,
                        principalTable: "YayinBilgisi",
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
                name: "IX_PersonelYayinBilgileris_YayinBilgisiId",
                table: "PersonelYayinBilgileris",
                column: "YayinBilgisiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonelYayinBilgileris");

            migrationBuilder.DropTable(
                name: "Personels");

            migrationBuilder.DropTable(
                name: "YayinBilgisi");

            migrationBuilder.DropTable(
                name: "AnaBilimDals");
        }
    }
}
