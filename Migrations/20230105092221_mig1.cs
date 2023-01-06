using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaramaMVC.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnaBilimDalis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnaBilimDalis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Personels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SurName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AId = table.Column<int>(type: "int", nullable: false),
                    AnaBilimDaliId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Personels_AnaBilimDalis_AnaBilimDaliId",
                        column: x => x.AnaBilimDaliId,
                        principalTable: "AnaBilimDalis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Personels_AnaBilimDaliId",
                table: "Personels",
                column: "AnaBilimDaliId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Personels");

            migrationBuilder.DropTable(
                name: "AnaBilimDalis");
        }
    }
}
